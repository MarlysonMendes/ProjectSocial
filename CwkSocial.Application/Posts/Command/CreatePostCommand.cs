using CwkSocial.Application.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Posts.Command
{
    public class CreatePostCommand : IRequest<OperationResult<Post>>
    {
        public Guid UserProfileId  { get; set; }
        public string TextContent { get; set; }
    }
}
