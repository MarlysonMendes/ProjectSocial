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
                if (post == null)
                {
                    result.IsError = true;
                    var error = new Error { Code = ErrorCode.NotFound, Message = $"No post found with Id {request.PostId}" };
                    result.Errors.Add(error);
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
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
