namespace SurveyBasket.Api.Contracts.Polls;

public record PollResponse(
    int Id ,
     string Title,
     string Summary,
     bool IsPublished,
     DateOnly StarstAt,
     DateOnly EndstAt
    );

