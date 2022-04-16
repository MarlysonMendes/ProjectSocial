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

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound,
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId));

                    return result;
                }

                if (post.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCode.PostDeleteNotPossible, PostsErrorMessages.PostDeleteNotPossible);
                    return result;
                }

                _ctx.Posts.Remove(post);
                await _ctx.SaveChangesAsync();

                result.Payload = post;
            }
           
            catch (Exception e)
            {
                result.AddError(ErrorCode.UnknownError, e.Message);
            }

            return result;

        }
    }
}
