using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Domain.Aggregates.PostAggregate
{
    public class Post
    {
        // crio a lista das minhas entidades para uso interno do meu dominio rico

        private readonly List<PostComment> _comments = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();

        public Post()
        {
        }
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }

        // Recomendado usar Enumerable, pois List e Collection possui metodos para alterar diretamente os valores

        public IEnumerable<PostComment> Commentes { get { return _comments; } }
        public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }

        //Factories
        public static Post CreatePost(Guid userProfileId, string textContent)
        {
           
            return new Post
            {
                UserProfileId = userProfileId,
                TextContent = textContent,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            };

        }

        //Public methods
        public void UpdatePostText(string newText)
        {
            TextContent = newText;
            LastModified = DateTime.UtcNow;
        }

        public void AddPostComment(PostComment newComment)
        {
            _comments.Add(newComment);
        }
        public void RemoveComment(PostComment toRemove)
        {
            _comments.Remove(toRemove);
        }



        public void AddInteraction(PostInteraction newInteraction)
        {
            _interactions.Add(newInteraction);
        }

        public void RemoveInteraction(PostInteraction toRemove)
        {
            _interactions.Remove(toRemove);
        }

    }
}
