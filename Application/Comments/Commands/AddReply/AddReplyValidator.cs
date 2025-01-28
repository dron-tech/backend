using Application.Common;
using FluentValidation;

namespace Application.Comments.Commands.AddReply;

public class AddReplyValidator : AbstractValidator<AddReplyCmd>
{
    public AddReplyValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.CommentId)
            .GreaterThan(0);

        RuleFor(x => x.Value)
            .NotEmpty()
            .MaximumLength(ValidateConstant.MaxCommentLength);
    }
}
