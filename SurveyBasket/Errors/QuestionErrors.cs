namespace SurveyBasket.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound = new(
        "Question.NotFound",
        "The specified question was not found."
    );
    public static readonly Error QuestionAlreadyExists = new(
        "Question.AlreadyExists",
        "A question with the same content already exists in this poll."
    );
}
