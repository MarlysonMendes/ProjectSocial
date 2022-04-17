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
                    => dest.Author.FullName, opt 
                    => opt.MapFrom( src 
                    => src.UserProfile.BasicInfo.FirstName + " " + src.UserProfile.BasicInfo.LastName))
                .ForMember(dest 
                    => dest.Author.City, opt 
                    => opt.MapFrom(src
                    => src.UserProfile.BasicInfo.CurrentCity))
                .ForMember(dest
                    => dest.Author.UserProfileId, opt
                    => opt.MapFrom(src
                    => src.UserProfile.UserProfileId))
                .ForMember(dest
                    => dest.Type, opt
                    => opt.MapFrom(src
                    => src.InteractionType.ToString());
        }
    }
}
