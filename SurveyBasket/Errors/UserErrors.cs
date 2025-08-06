namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredential = 
        new("User.InvalidCredential", "Invalid Email/Password",StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidTokensCredential = 
        new ("Token.InvalidCredential", "Invalid Token/RefreshToken",StatusCodes.Status401Unauthorized);

    public static readonly Error EmailAlreadyExists = 
        new ("Email.AlreadyExists", "Email AlreadyExists", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed =
        new("User.EmailNotConfirmed", "Email Not Confirmed", StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidCode =
        new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
    public static readonly Error IsConfirmedBefore =
        new("User.IsConfirmedBefore", "Is Confirmed Before", StatusCodes.Status401Unauthorized);



}
