using Application.Common;
using FluentValidation;

namespace Application.Comments.Commands.AddComment;

public class AddCommentValidator : AbstractValidator<AddCommentCmd>
{
    public AddCommentValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.EntityId)
            .GreaterThan(0);

        RuleFor(x => x.Value)
            .NotEmpty()
            .MaximumLength(ValidateConstant.MaxCommentLength);
    }
}
