using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Options;
using CwkSocial.Application.Services;
using CwkSocial.Dal;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Identity.CommandHandlers
{
    public class RegisterIdentityCommandHandler : IRequestHandler<RegisterIdentityCommand, OperationResult<string>>
    {
        private readonly DataContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IdentityService _identityService;

        public RegisterIdentityCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager, IdentityService identityService)
        {
            _ctx = ctx; 
            _userManager = userManager;
            _identityService = identityService;
        }

        public async Task<OperationResult<string>> Handle(RegisterIdentityCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();
            try
            {

                await ValidateIdentityDoesNotExist(result, request);
                if (result.IsError) return result;
               
                using var transaction = _ctx.Database.BeginTransaction();

                var identity = await CreateIdentityUserAsync(result, request, transaction);
                if (result.IsError) return result;


                var profile = await CreateUserProfileAsync(result, request, transaction, identity);
                await transaction.CommitAsync();

                result.Payload = GetJwtString(identity, profile);

                return result;
            }
            catch (UserProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(e => result.AddError(ErrorCode.ValidationError, ex.Message));
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknownError,ex.Message);
            }

            return result;
        }

        private async Task ValidateIdentityDoesNotExist(OperationResult<string> result, RegisterIdentityCommand request)
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

            if (existingIdentity != null)
                result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessages.IdentityUserAlreadyExists);

        }
        private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result, RegisterIdentityCommand request,
                IDbContextTransaction transaction)
        {
            var identity = new IdentityUser { Email = request.UserName, UserName = request.UserName };

            var createIdentity = await _userManager.CreateAsync(identity, request.Password);

            if (!createIdentity.Succeeded)
            {
                await transaction.RollbackAsync();

                foreach (var identityError in createIdentity.Errors)
                {
                    result.AddError(ErrorCode.IdentityUserCreateFailed, identityError.Description);
                }
            }
            return identity;
        }

        private async Task<UserProfile> CreateUserProfileAsync(OperationResult<string> result, RegisterIdentityCommand request, 
                IDbContextTransaction transaction, IdentityUser identity)
        {
            
            try
            {
                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.UserName,
                    request.Phone, request.DateOfBirth, request.CurrentCity);

                var profile = UserProfile.CreateUserProfile(identity.Id, basicInfo);
                _ctx.UserProfiles.Add(profile);
                await _ctx.SaveChangesAsync();
                return profile;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
    
        private string GetJwtString(IdentityUser identity, UserProfile profile)
        {

            var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                    new Claim("IdentityId",identity.Id),
                    new Claim("UserProfileId", profile.UserProfileId.ToString())
                });

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            return _identityService.WriteToken(token);

        }
    }
}
