using Application.Comments.Queries.GetComments;
using Application.Common.DTO;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(ContextDb context) : base(context)
    {
    }

    public Task<int> GetCommentCount(int entityId, ContentType type)
    {
        return type switch
        {
            ContentType.Comment => Entities.CountAsync(x => x.ReplyId == entityId),
            ContentType.Nft => Entities.CountAsync(x => x.NftId == entityId),
            ContentType.Video => Entities.CountAsync(x => x.VideoId == entityId),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    public async Task<PagedList<Comment>> GetPagedList(int entityId, ContentType type, int pgIndex,
        int pgSize)
    {
        var qry = type switch
        {
            ContentType.Comment => Entities.Where(x => x.ReplyId == entityId),
            ContentType.Nft => Entities.Where(x => x.NftId == entityId),
            ContentType.Video => Entities.Where(x => x.VideoId == entityId),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        qry = qry.OrderByDescending(x => x.CreatedAt);
        
        var count = await GetCommentCount(entityId, type);
        var skip = (pgIndex - 1) * pgSize;

        var comments = await qry
            .Skip(skip)
            .Take(pgSize)
            .ToArrayAsync();

        return new PagedList<Comment>
        {
            Items = comments,
            PageIndex = pgIndex,
            PageSize = pgSize,
            TotalCount = count,
            TotalPages = (int)Math.Ceiling(count / (double)pgSize)
        };
    }
}
