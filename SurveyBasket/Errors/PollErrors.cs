namespace SurveyBasket.Errors;

public static class   PollErrors

{
    public static readonly Error PollNotFound =
        new Error("Poll.NotFound", "Poll with this id NotFound");
    public static readonly Error PollTitleAlreadyExists =
        new Error("Poll.TitleAlreadyExists", "Poll Title Already Exists");
}
