using Application.Common.DTO;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class NftRepository : BaseRepository<Nft>, INftRepository
{
    public NftRepository(ContextDb context) : base(context)
    {
    }

    public Task<bool> ExistUserNftWithSameUrl(string url, int userId)
    {
        return Entities.AnyAsync(x => x.Url == url && x.UserId == userId);
    }
    
    public async Task<PagedList<Nft>> GetUserNftList(int userId, int pgIndex, int pgSize)
    {
        var qry = Entities
            .Include(x => x.PublishOptions)
            .Where(x => x.UserId == userId);
        
        var count = await qry.CountAsync();
        
        var skip = (pgIndex - 1) * pgSize;
        var nfts = await qry
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Take(pgSize)
            .ToArrayAsync();
        
        return new PagedList<Nft>
        {
            Items = nfts,
            PageIndex = pgIndex,
            PageSize = pgSize,
            TotalCount = count,
            TotalPages = (int)Math.Ceiling(count / (double)pgSize)
        };
    }

    public Task<Nft?> GetUserNft(int nftId, int userId)
    {
        return Entities.FirstOrDefaultAsync(x => x.Id == nftId && x.UserId == userId);
    }

    public Task<bool> IsNftExist(int nftId)
    {
        return Entities.AnyAsync(x => x.Id == nftId);
    }

    public Task<Nft[]> GetExistNftUrls(int userId, string[] urls)
    {
        return Entities
            .Where(x => x.UserId == userId && urls.Contains(x.ExplorerUrl))
            .ToArrayAsync();
    }

    public Task<int> GetUserNftCount(int userId)
    {
        return Entities.CountAsync(x => x.UserId == userId);
    }
}
