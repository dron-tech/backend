using Application.Common;
using FluentValidation;

namespace Application.Users.Commands.ThirdPartyLoginUser;

public class ThirdPartyLoginUserValidator : AbstractValidator<ThirdPartyLoginUserCmd>
{
    public ThirdPartyLoginUserValidator()
    {
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.AccessToken).NotEmpty();
        RuleFor(x => x.Platform).IsInEnum();
    }
}
