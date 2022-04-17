using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Domain.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        public Guid InteractionId { get; private set; }
        public Guid PostId { get; private set; }
        public Guid? UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public InteractionType InteractionType { get; private set; }


        //Factories
        public static PostInteraction CreatePostInteraction(Guid PostId, InteractionType type)
        {
            return new PostInteraction
            {
                PostId = PostId,
                InteractionType = type

            };
        }
    }
}
