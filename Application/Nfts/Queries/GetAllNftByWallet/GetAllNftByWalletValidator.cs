using Application.Common;
using FluentValidation;

namespace Application.Nfts.Queries.GetAllNftByWallet;

public class GetAllNftByWalletValidator : AbstractValidator<GetAllNftByWalletQuery>
{
    public GetAllNftByWalletValidator()
    {
        RuleFor(x => x.Address)
            .Must(x => x is { Length: ValidateConstant.EthAddressLength })
            .WithMessage(ValidateConstant.EthAddressLengthFailMsg);
    }
}
