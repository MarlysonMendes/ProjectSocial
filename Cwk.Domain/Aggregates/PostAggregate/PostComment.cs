using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Domain.Aggregates.PostAggregate
{
    public class PostComment
    {
        private PostComment()
        { }
        public Guid CommentId { get; private set; }
        public Guid PostId { get; private set; }
        public string Text { get; private set; }
        public Guid UserProfileId { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime LastModifed { get; private set; }

        public static PostComment CreatePostComment(Guid postId, string text, Guid userProfileId)
        {
            return new PostComment
            {
                PostId = postId,
                Text = text,
                UserProfileId = userProfileId,
                DateCreated = DateTime.Now,
                LastModifed = DateTime.Now
            };
        }

        public void UpdateCommentText(string newText)
        {
            Text = newText;
            LastModifed = DateTime.UtcNow;
        }

    }
}
