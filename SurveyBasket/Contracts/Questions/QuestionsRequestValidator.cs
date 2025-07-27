namespace SurveyBasket.Contracts.Questions;

public class QuestionsRequestValidator : AbstractValidator<QuestionsRequest>
{
    public QuestionsRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .Length(3, 1000);

        RuleFor(x => x.Answers)
            .NotNull();

        RuleFor(x => x.Answers)
            .Must(x => x.Count > 1)
            .WithMessage("At least two answers are required.")
            .When(x => x.Answers != null);

        RuleFor(x => x.Answers)
            .NotNull()
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You can not add duplicated Answers for The Same Question.")
            .When(x => x.Answers != null);

    }

}
