
using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Options;
using CwkSocial.Application.Services;
using CwkSocial.Dal;
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

namespace CwkSocial.Application.Identity.Handlers
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
               

                var identityUser = await _userManager.FindByEmailAsync(request.UserName);
                
                if (identityUser is null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = ErrorCode.IdentityUserDoesNotExist,
                        Message = $"Unable to find a user with the specifed username"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

                if (!validPassword)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = ErrorCode.IncorrectPassword,
                        Message = $"The provided password incorrect"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                var userProfile = await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id);

                var claimsIdentity = new ClaimsIdentity(new Claim[]
                 {
                    new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                    new Claim("IdentityId",identityUser.Id),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString())
                 });

                var token = _identityService.CreateSecurityToken(claimsIdentity);
                result.Payload = _identityService.WriteToken(token);
                return result;


            }
            catch (Exception ex)
            {
                result.IsError= true;
                var error = new Error
                {
                    Code = ErrorCode.UnknownError,
                    Message = $"{ex.Message}"
                };
                result.Errors.Add(error);
            }
            return result;
        }
    }



}
