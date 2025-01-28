using Application.Users.Commands.Login;
using FluentValidation;

namespace Application.Users.Commands.Logout;

public class LogoutValidator : AbstractValidator<LogoutCmd>
{
    public LogoutValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}
