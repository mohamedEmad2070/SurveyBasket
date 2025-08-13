namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredential = 
        new("User.InvalidCredential", "Invalid Email/Password",StatusCodes.Status401Unauthorized);
    public static readonly Error DisabledUser = 
        new("User.DisabledUser", "Disabled User", StatusCodes.Status401Unauthorized);

    public static readonly Error UserNotFound =
    new("User.UserNotFound", "User Not Found", StatusCodes.Status404NotFound);

    public static readonly Error LockedOut = 
        new("User.LockedOut", "Locked Out please wait 5 min", StatusCodes.Status401Unauthorized);

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
    public static readonly Error InvalidRoles =
        new("User.InvalidRoles", "Invalid oles", StatusCodes.Status400BadRequest);



}
