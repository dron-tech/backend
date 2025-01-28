using Application.Common;
using Application.Common.DTO.Nfts;
using Application.Common.Utils;
using FluentValidation;

namespace Application.Nfts.Commands.AddManyNft;

public class AddManyNftValidator : AbstractValidator<AddManyNftCmd>
{
    public AddManyNftValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleForEach(x => x.Nfts)
            .SetValidator(new AddNftDtoValidator());

        RuleFor(x => x.Nfts)
            .NotEmpty()
            .Must(x => x.All(y => x.Count(z => z.Url == y.Url) == 1))
            .WithMessage("Contains duplicate values");
    }
}

public class AddNftDtoValidator : AbstractValidator<AddNftDto>
{
    public AddNftDtoValidator()
    {
        RuleFor(x => x.Desc)
            .MaximumLength(ValidateConstant.MaxNftDesc);

        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(NftUrlUtil.Validate)
            .WithMessage(ValidateConstant.NftLinkFailMsg);

        RuleFor(x => x.LikeStyle)
            .IsInEnum();

        RuleFor(x => x.CommentType)
            .IsInEnum();
    }
}
