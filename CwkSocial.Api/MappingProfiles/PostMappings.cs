using AutoMapper;
using CwkSocial.Api.Contracts.Posts.Responses;
using CwkSocial.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.MappingProfiles
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<Post, PostResponse>();
            CreateMap<PostComment, PostCommentResponse>();
        }
    }
}
