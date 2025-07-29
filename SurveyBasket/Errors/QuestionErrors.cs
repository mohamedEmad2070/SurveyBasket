namespace SurveyBasket.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound = new(
        "Question.NotFound",
        "The specified question was not found.", StatusCodes.Status404NotFound
    );
    public static readonly Error QuestionAlreadyExists = new(
        "Question.AlreadyExists",
        "A question with the same content already exists in this poll.",StatusCodes.Status409Conflict
    );
}
