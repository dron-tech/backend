using Application.Common;
using FluentValidation;

namespace Application.Users.Commands.ConfirmEmail;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCmd>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .LessThan(ValidateConstant.MaxResetCodeValue);

        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}
