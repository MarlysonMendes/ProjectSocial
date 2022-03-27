using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.UserProfiles.Queries
{
    public class GetAllUserProfileQuery : IRequest<OperationResult<IEnumerable<UserProfile>>>
    {
        
    }
}
