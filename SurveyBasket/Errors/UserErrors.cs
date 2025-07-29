namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredential = 
        new("User.InvalidCredential", "Invalid Email/Password",StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidTokensCredential = 
        new ("Token.InvalidCredential", "Invalid Token/RefreshToken",StatusCodes.Status401Unauthorized);
}
