using FluentValidation;

namespace Application.Users.Commands.ResendConfirmEmail;

public class ResendConfirmEmailValidator : AbstractValidator<ResendConfirmEmailCmd>
{
    public ResendConfirmEmailValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}
