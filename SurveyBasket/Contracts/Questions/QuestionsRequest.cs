namespace SurveyBasket.Contracts.Questions;

public record QuestionsRequest(
    string Content,
    List<string> Answers
    );

