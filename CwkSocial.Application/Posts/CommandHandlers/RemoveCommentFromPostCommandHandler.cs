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
    public class RemoveCommentFromPostCommandHandler : IRequestHandler<RemoveCommentFromPostCommand, OperationResult<PostComment>>
    {
        private readonly DataContext _ctx;
        private readonly OperationResult<PostComment> _result;

        public RemoveCommentFromPostCommandHandler(DataContext ctx)
        {
            _ctx = ctx;
            _result = new OperationResult<PostComment>();
        }
        public async Task<OperationResult<PostComment>> Handle(RemoveCommentFromPostCommand request, CancellationToken cancellationToken)
        {
            var post = await _ctx.Posts
            .Include(p => p.Commentes)
            .FirstOrDefaultAsync(p => p.PostId == request.PostId);

            if (post == null)
            {
                _result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostNotFound);
                return _result;
            }

            var comment = post.Commentes
                .FirstOrDefault(c => c.CommentId == request.CommentId);
            if (comment == null)
            {
                _result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostCommentNotFound);
                return _result;
            }

            if (comment.UserProfileId != request.UserProfileId)
            {
                _result.AddError(ErrorCode.CommentRemovalNotAuthorized,
                    PostsErrorMessages.CommentRemovalNotAuthorized);
                return _result;
            }

            post.RemoveComment(comment);
            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync();

            _result.Payload = comment;
            return _result;
        }
    }
}
