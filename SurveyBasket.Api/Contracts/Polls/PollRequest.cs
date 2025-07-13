namespace SurveyBasket.Api.Contracts.Polls;

public record PollRequest(
     string Title,
     string Summary,
     bool IsPublished,
     DateOnly StarstAt,
     DateOnly EndstAt
    );

