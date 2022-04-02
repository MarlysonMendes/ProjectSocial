using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Command;
using CwkSocial.Dal;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Posts.CommandHandlers
{
    public class UpdatePostTextCommandHandler : IRequestHandler<UpdatePostTextCommand, OperationResult<Post>>
    {
        private readonly DataContext _ctx;

        public UpdatePostTextCommandHandler(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<OperationResult<Post>> Handle(UpdatePostTextCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post == null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.NotFound,
                        Message = $"No post found with Id {request.PostId}"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                post.UpdatePostText(request.NewText);
                await _ctx.SaveChangesAsync();

                result.Payload = post;
                return result;

            }
            catch (PostNotValidException e)
            {
                result.IsError = true;
                e.ValidationErrors.ForEach(er =>
                {
                    var error = new Error { Code = ErrorCode.ValidationError, Message = $"{e.Message}" };
                    result.Errors.Add(error);
                });
            }
            catch (Exception e)
            {
                var error = new Error { Code = ErrorCode.UnknownError, Message = $"{e.Message}" };
                result.IsError = true;

            }
            
            return result;
        }
    }
}