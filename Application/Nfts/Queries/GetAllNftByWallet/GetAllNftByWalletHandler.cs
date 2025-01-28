using Application.Common.DTO.Nfts;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Nfts.Queries.GetAllNftByWallet;

public class GetAllNftByWalletHandler : IRequestHandler<GetAllNftByWalletQuery, UserErc20Nft[]>
{
    private readonly IMoralisService _moralisService;

    public GetAllNftByWalletHandler(IMoralisService moralisService)
    {
        _moralisService = moralisService;
    }

    public async Task<UserErc20Nft[]> Handle(GetAllNftByWalletQuery request, CancellationToken cancellationToken)
    {
        return await _moralisService.GetNftByWallet(request.Address);
    }
}
