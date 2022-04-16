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
    public class GetAllPostsHandler : IRequestHandler<GetAllPostsQuery, OperationResult<List<Post>>>
    {
        private readonly DataContext _ctx;

        public GetAllPostsHandler(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<OperationResult<List<Post>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<Post>>();
            try
            {
                var posts = await _ctx.Posts.ToListAsync();
                result.Payload = posts;
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.UnknownError, e.Message);
                return result;
            }

            return result;
        }
    }
}
