namespace CwkSocial.Api
{
    public static class ApiRoutes
    {
        public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

        public static class UserProfiles
        {
            public const string IdRoute = "{id}";
        }

        public static class Posts
        {
            public const string IdRoute = "{id}";
            public const string PostComments = "{postId}/comments";
            public const string CommentById = "{postId}/comments/{commentId}";
            public const string DeleteInteraction = "{postId}/interactions/{interactionsId}";
            public const string IntecationsById = "{postId}/interaction/{interactionId}";
            public const string PostIntecations = "{postId}/interaction";
           
        }
        public static class Identity
        {
            public const string login= "login";
            public const string Registration = "registration";
            public const string IdentityById = "{identityUserId}";
            public const string CurrentUser = "currentuser";
        }


    }
}