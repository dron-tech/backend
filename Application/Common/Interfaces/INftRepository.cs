using Application.Common.DTO;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface INftRepository : IRepository<Nft>
{
    public Task<bool> ExistUserNftWithSameUrl(string url, int userId);
    public Task<PagedList<Nft>> GetUserNftList(int userId, int pgIndex, int pgSize);
    public Task<Nft?> GetUserNft(int nftId, int userId);
    public Task<bool> IsNftExist(int nftId);
    public Task<Nft[]> GetExistNftUrls(int userId, string[] urls);
    public Task<int> GetUserNftCount(int userId);
}
