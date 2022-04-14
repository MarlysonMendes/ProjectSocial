using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Command;
using CwkSocial.Dal;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Posts.CommandHandlers
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, OperationResult<Post>>
    {
        private DataContext _ctx;
        public DeletePostCommandHandler(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<OperationResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
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

                if (post.UserProfileId != request.UserProfileId)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.PostDeleteNotPossible,
                        Message = $"Only the owner of a post can delete it"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                _ctx.Posts.Remove(post);
                await _ctx.SaveChangesAsync();

                result.IsError = false;
                result.Payload = post;
            }
           
            catch (Exception e)
            {
                var error = new Error { Code = ErrorCode.UnknownError, Message = $"{e.Message}" };
                result.IsError = true;
                result.Errors.Add(error);

            }

            return result;

        }
    }
}
