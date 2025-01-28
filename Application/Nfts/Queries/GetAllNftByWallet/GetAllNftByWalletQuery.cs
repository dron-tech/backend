using Application.Common.DTO.Nfts;
using MediatR;

namespace Application.Nfts.Queries.GetAllNftByWallet;

public class GetAllNftByWalletQuery : IRequest<UserErc20Nft[]>
{
    public string Address { get; set; } = string.Empty;
}
