namespace SurveyBasket.Contracts.Roles;

public class RoleRequestValidator:AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name).
            NotEmpty()
            .Length(3, 200);

        RuleFor(x => x.Permissions)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Permissions)
            .Must(x => x.Distinct().Count() == x.Count())
            .WithMessage("You can not add dublicated Permission")
            .When(x => x.Permissions != null);
    }
}
