﻿using CwkSocial.Application.Enums;
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
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
    {
        private readonly DataContext _ctx;
        public GetPostByIdHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();
            var post = await _ctx.Posts
                .FirstOrDefaultAsync(p => p.PostId == request.PostId);

            if (post is null)
            {
                result.IsError = true;
                var error = new Error { Code = ErrorCode.NotFound,
                            Message = $"No post found with Id {request.PostId}"};
                
                return result;
            }

            result.Payload = post;
            
            return result;
        }
    }
}
