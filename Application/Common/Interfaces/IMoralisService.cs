using Application.Common.DTO.Nfts;

namespace Application.Common.Interfaces;

public interface IMoralisService
{
    public Task<UserErc20Nft[]> GetNftByWallet(string address);
}
