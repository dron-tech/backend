using Application.Common;
using FluentValidation;

namespace Application.Users.Commands.RefreshUserToken;

public class RefreshUserTokenValidator : AbstractValidator<RefreshUserTokenCmd>
{
    public RefreshUserTokenValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .MinimumLength(ValidateConstant.MaxDefaultStrLength);
    }
}
