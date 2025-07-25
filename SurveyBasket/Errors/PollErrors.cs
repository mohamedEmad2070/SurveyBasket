namespace SurveyBasket.Errors;

public static class   PollErrors

{
    public static readonly Error PollNotFound =
        new Error("Poll.NotFound", "Poll with this id NotFound");
}
