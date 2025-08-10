namespace SurveyBasket.Errors;

public static class RoleErrors
{
    public static readonly Error RoleNotFound = 
        new("User.RoleNotFound", "Role Not Found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidPermissions =
        new("Role.InvalidPermissions", "Invalid InvalidPermissions", StatusCodes.Status400BadRequest);

    public static readonly Error RoleAlreadyExists =
        new("Role.AlreadyExists", "Role AlreadyExists", StatusCodes.Status409Conflict);

    //public static readonly Error EmailNotConfirmed =
    //    new("User.EmailNotConfirmed", "Email Not Confirmed", StatusCodes.Status401Unauthorized);
    //public static readonly Error InvalidCode =
    //    new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
    //public static readonly Error IsConfirmedBefore =
    //    new("User.IsConfirmedBefore", "Is Confirmed Before", StatusCodes.Status401Unauthorized);



}
