namespace SurveyBasket.Api.Contracts.Validation;

public class CreatePollRequestValidator: AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100);
        RuleFor(x => x.Summary)
          .NotEmpty()
          .Length(3, 1000);
    }
}
