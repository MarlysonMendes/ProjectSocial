﻿using CwkSocial.Application.Enums;
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
    internal class DeleteUserProfileHandler : IRequestHandler<DeleteUserProfile, OperationResult<UserProfile>>
    {
        private readonly DataContext _ctx;
        public DeleteUserProfileHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfile request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();   
            var userProfile = await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);
            
            if(userProfile == null)
            {
                result.IsErro = true;
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"No userProfile found with ID {request.UserProfileId}"
                };
                result.Erros.Add(error);
                return result;
            }



            _ctx.UserProfiles.Remove(userProfile);

            await _ctx.SaveChangesAsync();
            result.PayLoad = userProfile;

            return result;
        }
    }
}