using SurveyBasket.Contracts.Answers;

namespace SurveyBasket.Contracts.Questions;

public record QuestionsResponse(
    int Id,
    string Content,
    IEnumerable<AnswerResponse> Answers
    );

