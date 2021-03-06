using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.Dal;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.UserProfiles.QueryHandlers
{
    public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
    {
        private readonly DataContext _ctx;

        public GetUserProfileByIdQueryHandler(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            var userProfile =  await _ctx.UserProfiles
                .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);

            if (userProfile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            result.Payload = userProfile;
            return result;
        }
    }
}
