using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.Posts.Queries;
using CwkSocial.Dal;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Posts.QueriesHandlers
{
    public class GetPostInteractionsHandler : IRequestHandler<GetPostInteractionsQuery, OperationResult<List<PostInteraction>>>
    {
        private readonly DataContext _ctx;
        public GetPostInteractionsHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractionsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<PostInteraction>>();
            try
            {
                var post = await _ctx.Posts
                    .Include(p => p.Interactions)
                    .ThenInclude(i => i.UserProfile)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostNotFound);
                    return result;
                }
                 result.Payload = post.Interactions.ToList();
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.UnknownError, e.Message);
            }
            return result;
        }
    }
}
