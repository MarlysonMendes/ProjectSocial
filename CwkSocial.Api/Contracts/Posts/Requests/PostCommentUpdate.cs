using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.Posts.Requests
{
    public class PostCommsentUpdate
    {
        [Required]
        public string Text { get; set; }
    }
}
