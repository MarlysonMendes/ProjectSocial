using CwkSocial.Application.Enums;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles;
using CwkSocial.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Identity.CommandHandlers
{
    public class RemoveAccountCommandHandler : IRequestHandler<RemoveAccountCommand, OperationResult<bool>>
    {
        private readonly DataContext _ctx;

        public RemoveAccountCommandHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<OperationResult<bool>> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
        {
            {
                var result = new OperationResult<bool>();

                try
                {
                    var identityUser = await _ctx.Users.FirstOrDefaultAsync(iu
                        => iu.Id == request.IdentityUserId.ToString());

                    if (identityUser == null)
                    {
                        result.AddError(ErrorCode.IdentityUserDoesNotExist,
                            IdentityErrorMessages.NonExistentIdentityUser);
                        return result;
                    }

                    var userProfile = await _ctx.UserProfiles
                        .FirstOrDefaultAsync(up
                            => up.IdentityId == request.IdentityUserId.ToString());

                    if (userProfile == null)
                    {
                        result.AddError(ErrorCode.NotFound, UserProfilesErrorMessages.UserProfileNotFound);
                        return result;
                    }

                    if (identityUser.Id != request.RequestorGuid.ToString())
                    {
                        result.AddError(ErrorCode.UnauthorizedAccountRemoval,
                            IdentityErrorMessages.UnauthorizedAccountRemoval);

                        return result;
                    }

                    _ctx.UserProfiles.Remove(userProfile);
                    _ctx.Users.Remove(identityUser);
                    await _ctx.SaveChangesAsync();

                    result.Payload = true;
                }
                catch (Exception e)
                {
                    result.AddError(ErrorCode.UnknownError, e.Message);
                }

                return result;
            }
        }
    }
}
