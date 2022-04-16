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

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound,
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                    return result;
                }

                if(post.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCode.PostUpdateNotPossible, PostsErrorMessages.PostUpdateNotPossible);
                    return result;
                }

                post.UpdatePostText(request.NewText);
                await _ctx.SaveChangesAsync();

                result.Payload = post;
                return result;

            }
            catch (PostNotValidException e)
            {
                e.ValidationErrors.ForEach(er => result.AddError(ErrorCode.ValidationError, er));
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.UnknownError, e.Message);
            }

            return result;
        }
    }
}