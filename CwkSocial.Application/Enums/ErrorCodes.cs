namespace CwkSocial.Application.Enums
{
    public enum ErrorCode
    {
        NotFound = 404,
        ServerError = 500,

        // Validation errors should be in the range 100 - 199
        ValidationError = 101,
        //Infrastruture erros should be in range 200-299
        IdentityUserAlreadyExists = 201,
        IdentityUserCreateFailed = 202,
        IdentityUserDoesNotExist = 203,
        IncorrectPassword = 204,
        InexistenUserProfile = 205,

        //Application error
        PostUpdateNotPossible = 300,
        PostDeleteNotPossible = 301,
        InteractionRemovalNotAuthorized = 302,
        UnknownError = 999,
        UnauthorizedAccountRemoval = 1000,
        CommentRemovalNotAuthorized = 1001
    }
}
