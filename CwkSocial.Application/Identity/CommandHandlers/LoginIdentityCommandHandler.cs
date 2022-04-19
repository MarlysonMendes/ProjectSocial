
using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Options;
using CwkSocial.Application.Services;
using CwkSocial.Dal;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class LoginIdentityCommandHandler : IRequestHandler<LoginIdentityCommand, OperationResult<string>>
    {

        private readonly DataContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;


        public LoginIdentityCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager, IdentityService identityService )
        {
            _ctx = ctx;
            _userManager = userManager;
            _identityService = identityService;            
        }

        public async Task<OperationResult<string>> Handle(LoginIdentityCommand request, CancellationToken cancellationToken)
        {

            var result = new OperationResult<string>();
            try
            {

                var identityUser = await ValidateAndIdentityUserAsync(request, result);
                if (result.IsError) return result;


                var userProfile = await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id);

                if(userProfile is null)
                {
                    result.AddError(ErrorCode.InexistenUserProfile, 
                        IdentityErrorMessages.NonExistentIdentityUser);
                    return null;
                }


                result.Payload = GetJwtString(identityUser, userProfile);

                return result;

            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.UnknownError, ex.Message);
            }
            return result;
        }

        private async Task<IdentityUser?> ValidateAndIdentityUserAsync(LoginIdentityCommand request, OperationResult<string> result)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.UserName);

            if (identityUser is null)
                result.AddError(ErrorCode.InexistenUserProfile,
                    IdentityErrorMessages.NonExistentIdentityUser);


            var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!validPassword)
                result.AddError(ErrorCode.IncorrectPassword, "The provided password incorrect");
 

            return identityUser;
        }
    
        private string GetJwtString ( IdentityUser identityUser, UserProfile userProfile)
        {

            var claimsIdentity = new ClaimsIdentity(new Claim[]
             {
                    new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                    new Claim("IdentityId",identityUser.Id),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString())
             });

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            return _identityService.WriteToken(token);
        }
    
    }



}
