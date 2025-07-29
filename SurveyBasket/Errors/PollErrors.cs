namespace SurveyBasket.Errors;

public static class   PollErrors

{
    public static readonly Error PollNotFound =
        new("Poll.NotFound", "Poll with this id NotFound",StatusCodes.Status404NotFound);
    public static readonly Error PollTitleAlreadyExists =
        new("Poll.TitleAlreadyExists", "Poll Title Already Exists", StatusCodes.Status409Conflict);
}
