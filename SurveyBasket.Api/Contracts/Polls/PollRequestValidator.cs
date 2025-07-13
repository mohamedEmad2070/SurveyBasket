namespace SurveyBasket.Api.Contracts.Polls;

public class LoginRequestValidator: AbstractValidator<PollRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100);
        RuleFor(x => x.Summary)
          .NotEmpty()
          .Length(3, 1000);
        RuleFor(x => x.StarstAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
        RuleFor(x=>x).Must(HasValidEndDate)
            .WithName(nameof(PollRequest.EndstAt)).
            WithMessage("{PropertyName} must be greater than or equals Start Date");
    }

    private bool HasValidEndDate(PollRequest pollRequest)
    {
        return pollRequest.EndstAt >= pollRequest.StarstAt;
    }
}
