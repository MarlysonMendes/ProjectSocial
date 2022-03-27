using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.Dal;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.UserProfiles.CommandHandlers
{
    public class UpdateUserProfileBasicInfoHandler : IRequestHandler<UpdateUserProfileBasicInfo, OperationResult<UserProfile>>
    {
        private readonly DataContext _ctx;

        public UpdateUserProfileBasicInfoHandler(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfo request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();


            try 
            {
                var userProfile = await _ctx.UserProfiles
                    .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);
                
                if(userProfile is null)
                {
                    result.IsErro = true;
                    var error = new Error { Code = ErrorCode.NotFound, 
                        Message = $"No UserProfile found with ID {request.UserProfileId} " };
                    result.Erros.Add(error);
                    return result;
                }
                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress,
                    request.Phone, request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _ctx.UserProfiles.Update(userProfile);
                await _ctx.SaveChangesAsync();
                result.PayLoad = userProfile;
                return result;
            }
            catch (Exception ex)
            {
                var error = new Error { Code = ErrorCode.ServerError, Message = ex.Message };
                result.IsErro = true;
                result.Erros.Add(error);
            }

            return result;
        }
    }
}
