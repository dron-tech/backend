using Application.Common;
using FluentValidation;

namespace Application.Users.Commands.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .MaximumLength(ValidateConstant.MaxDefaultStrLength);

        RuleFor(x => x.Psw)
            .MinimumLength(ValidateConstant.MinPswLength)
            .MaximumLength(ValidateConstant.MaxDefaultStrLength)
            .Matches(ValidateConstant.PswRegPattern)
            .WithMessage(ValidateConstant.PswRegFailMessage);

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(ValidateConstant.MaxDefaultStrLength);
    }
}
