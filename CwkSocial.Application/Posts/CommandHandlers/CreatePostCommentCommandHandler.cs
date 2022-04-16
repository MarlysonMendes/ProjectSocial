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
    public class CreatePostCommentCommandHandler : IRequestHandler<CreatePostCommentCommand, OperationResult<PostComment>>
    {

        private readonly DataContext _ctx;
        public CreatePostCommentCommandHandler(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<OperationResult<PostComment>> Handle(CreatePostCommentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();
            try
            {
                var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);
                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound,
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                    return result;
                }
                var comment = PostComment.CreatePostComment(request.PostId, request.CommentText, request.UserProfileId);
            
                post.AddPostComment(comment);

                _ctx.Posts.Update(post);
                await _ctx.SaveChangesAsync();

                result.Payload = comment;
            }
            catch (PostCommentNotValidException e)
            {
                e.ValidationErrors.ForEach(er => result.AddError(ErrorCode.ValidationError, er));
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.UnknownError,e.Message);
            }

            return result;
        }
    }
}
