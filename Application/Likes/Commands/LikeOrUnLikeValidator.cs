using FluentValidation;

namespace Application.Likes.Commands;

public class LikeOrUnLikeValidator : AbstractValidator<LikeOrUnLikeCmd>
{
    public LikeOrUnLikeValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}
