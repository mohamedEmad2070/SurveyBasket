using SurveyBasket.Abstractions.Consts;

namespace SurveyBasket.Contracts.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Minimum eight characters, at least one upper case English letter, one lower case English letter, one number and one special character");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3,100);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3,100);
    }
}
