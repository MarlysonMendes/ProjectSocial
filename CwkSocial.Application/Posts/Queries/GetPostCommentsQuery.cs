using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Posts.Queries
{
    public class GetPostCommentsQuery : IRequest<OperationResult<List<PostComment>>>
    {
        public Guid PostId { get; set; }
    }
}
