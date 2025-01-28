using FluentValidation;

namespace Application.Users.Commands.LoginByApple;

public class LoginByAppleValidator : AbstractValidator<LoginByAppleCmd>
{
    public LoginByAppleValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);
    }
}
