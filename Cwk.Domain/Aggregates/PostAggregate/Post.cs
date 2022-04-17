﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Exceptions;
using CwkSocial.Domain.Validators.PostValidators;

namespace CwkSocial.Domain.Aggregates.PostAggregate
{
    public class Post
    {
        // crio a lista das minhas entidades para uso interno do meu dominio rico

        private readonly List<PostComment> _comments = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();

        private Post()
        {
        }
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }

        // Recomendado usar Enumerable, pois List e Collection possui metodos para alterar diretamente os valores

        public IEnumerable<PostComment> Commentes { get { return _comments; } }
        public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }

        //Factories
        /// <summary>
        /// Creates a new post instance
        /// </summary>
        /// <param name="userProfileId">User profile ID</param>
        /// <param name="textContent">Post content</param>
        /// <returns><see cref="Post"/></returns>
        /// <exception cref="PostNotValidException"></exception>
        public static Post CreatePost(Guid userProfileId, string textContent)
        {
            var validator = new PostValidator();
            var objectToValidate = new Post
            {
                UserProfileId = userProfileId,
                TextContent = textContent,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            };

            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new PostNotValidException("Post is not valid");
            validationResult.Errors.ForEach(vr => exception.ValidationErrors.Add(vr.ErrorMessage));
            throw exception;
        }

        //Public methods
        /// <summary>
        /// Updates the post text
        /// </summary>
        /// <param name="newText">The updated post text</param>
        /// <exception cref="PostNotValidException"</exception>
        public void UpdatePostText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                var exception = new PostNotValidException("Cannot update post" +
                                                           "Post text is not valid");

                exception.ValidationErrors.Add("The provided text is either null or contains onlu white space");
                throw exception;
            }

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
