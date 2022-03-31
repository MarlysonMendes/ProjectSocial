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
    public class GetAllUserProfilesQueryHandler 
        : IRequestHandler<GetAllUserProfileQuery, OperationResult<IEnumerable<UserProfile>>>
    {
        private readonly DataContext _ctx;
        public GetAllUserProfilesQueryHandler(DataContext ctx)
        {
            _ctx = ctx;
        }


        public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfileQuery request, CancellationToken cancellationToken)
        {  
            var result = new OperationResult<IEnumerable<UserProfile>>();

            result.Payload =  await _ctx.UserProfiles.ToListAsync();
            return result;
        }
    }
}
