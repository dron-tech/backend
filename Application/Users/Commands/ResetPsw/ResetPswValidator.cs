using Application.Common;
using FluentValidation;

namespace Application.Users.Commands.ResetPsw;

public class ResetPswValidator : AbstractValidator<ResetPswCmd>
{
    public ResetPswValidator()
    {
        RuleFor(x => x.EmailOrLogin)
            .NotEmpty()
            .MaximumLength(ValidateConstant.MaxDefaultStrLength);

        RuleFor(x => x.Psw)
            .MinimumLength(ValidateConstant.MinPswLength)
            .MaximumLength(ValidateConstant.MaxDefaultStrLength)
            .Matches(ValidateConstant.PswRegPattern)
            .WithMessage(ValidateConstant.PswRegFailMessage);

        RuleFor(x => x.Code)
            .NotEmpty()
            .LessThan(ValidateConstant.MaxResetCodeValue);
    }
}
