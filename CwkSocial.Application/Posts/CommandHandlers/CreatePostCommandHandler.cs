using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Command;
using CwkSocial.Dal;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Posts.CommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, OperationResult<Post>>
    {
        private readonly DataContext _ctx;
        public CreatePostCommandHandler(DataContext ctx)
        {
            _ctx = ctx;
        }


        public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();
            try
            {
                var post = Post.CreatePost(request.UserProfileId, request.TextContent);
                _ctx.Posts.Add(post);
                await _ctx.SaveChangesAsync();

                result.Payload = post;
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
                var error = new Error { Code = ErrorCode.UnknownError, Message = $"{e.Message}"};
                result.IsError = true;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
