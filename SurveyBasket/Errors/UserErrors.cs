namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredential = 
        new Error("User.InvalidCredential", "Invalid Email/Password");

    public static readonly Error InvalidTokensCredential = 
        new Error("Token.InvalidCredential", "Invalid Token/RefreshToken");
}
