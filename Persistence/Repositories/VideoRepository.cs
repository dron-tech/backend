using Application.Common.DTO;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class VideoRepository : BaseRepository<Video>, IVideoRepository
{
    public VideoRepository(ContextDb context) : base(context)
    {
    }

    public Task<Video?> GetUserVideoById(int videoId, int userId)
    {
        return Entities.FirstOrDefaultAsync(x => x.Id == videoId && x.UserId == userId);
    }

    public async Task<PagedList<Video>> GetUserVideoList(int userId, int pgIndex, int pgSize)
    {
        var qry = Entities
            .Where(x => x.UserId == userId)
            .Include(x => x.PublishOptions);
        
        var count = await qry.CountAsync();
        
        var skip = (pgIndex - 1) * pgSize;
        var videos = await qry
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Take(pgSize)
            .ToArrayAsync();
        
        return new PagedList<Video>
        {
            Items = videos,
            PageIndex = pgIndex,
            PageSize = pgSize,
            TotalCount = count,
            TotalPages = (int)Math.Ceiling(count / (double)pgSize)
        };
    }

    public Task<bool> IsVideoExist(int videoId)
    {
        return Entities.AnyAsync(x => x.Id == videoId);
    }

    public Task<int> GetUserVideoCount(int userId)
    {
        return Entities.CountAsync(x => x.UserId == userId);
    }
}
