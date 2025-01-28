using Application.Common;
using FluentValidation;

namespace Application.Users.Commands.Login;

public class LoginCmdValidator : AbstractValidator<LoginCmd>
{
    public LoginCmdValidator()
    {
        RuleFor(x => x.EmailOrLogin)
            .NotEmpty()
            .MaximumLength(ValidateConstant.MaxDefaultStrLength);

        RuleFor(x => x.Psw)
            .NotEmpty()
            .MaximumLength(ValidateConstant.MaxDefaultStrLength);
    }
}
