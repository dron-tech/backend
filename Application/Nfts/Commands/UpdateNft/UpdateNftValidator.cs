using Application.Common;
using FluentValidation;

namespace Application.Nfts.Commands.UpdateNft;

public class UpdateNftValidator : AbstractValidator<UpdateNftCmd>
{
    public UpdateNftValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.NftId)
            .GreaterThan(0);
        
        RuleFor(x => x.Desc)
            .MaximumLength(ValidateConstant.MaxNftDesc);

        RuleFor(x => x.LikeStyle)
            .IsInEnum();

        RuleFor(x => x.CommentType)
            .IsInEnum();
    }
}
