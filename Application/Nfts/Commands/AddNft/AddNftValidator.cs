using Application.Common;
using Application.Common.Utils;
using FluentValidation;

namespace Application.Nfts.Commands.AddNft;

public class AddNftValidator : AbstractValidator<AddNftCmd>
{
    public AddNftValidator()
    {
        RuleFor(x => x.Desc)
            .MaximumLength(ValidateConstant.MaxNftDesc);

        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(NftUrlUtil.Validate)
            .WithMessage(ValidateConstant.NftLinkFailMsg);

        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.LikeStyle)
            .IsInEnum();

        RuleFor(x => x.CommentType)
            .IsInEnum();
    }
}
