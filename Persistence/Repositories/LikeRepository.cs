using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class LikeRepository : BaseRepository<Like>, ILikeRepository
{
    public LikeRepository(ContextDb context) : base(context)
    {
    }

    public Task<Like?> GetUserLike(int userId, int entityId, ContentType type)
    {
        var qry = Entities.Where(x => x.UserId == userId);
        return type switch
        {
            ContentType.Comment => qry.FirstOrDefaultAsync(x => x.CommentId == entityId),
            ContentType.Nft => qry.FirstOrDefaultAsync(x => x.NftId == entityId),
            ContentType.Video => qry.FirstOrDefaultAsync(x => x.VideoId == entityId),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    public Task<int> GetLikeCount(int entityId, ContentType type)
    {
        return type switch
        {
            ContentType.Comment => Entities.CountAsync(x => x.CommentId == entityId),
            ContentType.Nft => Entities.CountAsync(x => x.NftId == entityId),
            ContentType.Video => Entities.CountAsync(x => x.VideoId == entityId),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    public Task<bool> CheckExist(int userId, int entityId, ContentType type)
    {
        var qry = Entities.Where(x => x.UserId == userId);
        return type switch
        {
            ContentType.Comment => qry.AnyAsync(x => x.CommentId == entityId),
            ContentType.Nft => qry.AnyAsync(x => x.NftId == entityId),
            ContentType.Video => qry.AnyAsync(x => x.VideoId == entityId),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
