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

        UnknownError = 999

    }
}
