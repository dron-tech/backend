using Application.Common;
using FluentValidation;

namespace Application.Users.Commands.SendResetPswCode;

public class SendResetPswCodeValidator : AbstractValidator<SendResetPswCodeCmd>
{
    public SendResetPswCodeValidator()
    {
        RuleFor(x => x.EmailOrLogin)
            .NotEmpty()
            .MaximumLength(ValidateConstant.MaxDefaultStrLength);
    }
}
