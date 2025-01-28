using FluentValidation;

namespace Application.Nfts.Commands.DeleteNft;

public class DeleteNftValidator : AbstractValidator<DeleteNftCmd>
{
    public DeleteNftValidator()
    {
        RuleFor(x => x.NftId)
            .GreaterThan(0);

        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}
