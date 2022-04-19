using AutoMapper;
using CwkSocial.Api.Contracts.Posts.Responses;
using CwkSocial.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PostInteraction = CwkSocial.Domain.Aggregates.PostAggregate.PostInteraction;

namespace CwkSocial.Application.MappingProfiles
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<Post, PostResponse>();
            CreateMap<PostComment, PostCommentResponse>();
            CreateMap<PostInteraction, CwkSocial.Api.Contracts.Posts.Responses.PostInteraction>()
            .ForMember(dest
                => dest.Type, opt
                => opt.MapFrom(src
                => src.InteractionType.ToString()))
            .ForMember(dest => dest.Author, opt
            => opt.MapFrom(src => src.UserProfile));
        }
    }
}
