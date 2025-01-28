using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Common.Enums;
using Domain.Entities;
using MediatR;

namespace Application.Comments.Commands.AddReply;

public class AddReplyHandler : IRequestHandler<AddReplyCmd, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddReplyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddReplyCmd request, CancellationToken cancellationToken)
    {
        var comment = await _unitOfWork.CommentRepository.GetById(request.CommentId);
        if (comment is null)
        {
            throw new NotFoundException("Comment not found");
        }

        await EnsureCanAddReply(comment, request.UserId);

        var reply = new Comment
        {
            Value = request.Value,
            UserId = request.UserId,
            ReplyId = comment.Id,
        };

        await _unitOfWork.CommentRepository.Insert(reply);
        await _unitOfWork.CommitAsync(cancellationToken);

        return reply.Id;
    }

    private async Task EnsureCanAddReply(Comment comment, int userId)
    {
        var publishOptsId = 0;
        var entityOwnerId = 0;
        
        if (comment.NftId is { } nftId)
        {
            var nft = await _unitOfWork.NftRepository.GetByIdOrFail(nftId);

            publishOptsId = nft.PublishOptionsId;
            entityOwnerId = nft.UserId;
        }

        if (comment.VideoId is { } videoId)
        {
            var video = await _unitOfWork.VideoRepository.GetByIdOrFail(videoId);
            
            publishOptsId = video.PublishOptionsId;
            entityOwnerId = video.UserId;
        }

        if (comment.ReplyId is { })
        {
            throw new BadRequestException("Can't add comment to reply comment");
        }
        
        var opts = await _unitOfWork.PublishOptionsRepository.GetByIdOrFail(publishOptsId);
        if (opts.CommentType is CommentType.AllowEveryone)
        {
            return;
        }

        if (opts.CommentType is CommentType.Disable)
        {
            throw new BadRequestException("Comments is disable");
        }

        if (userId == entityOwnerId)
        {
            return;
        }

        var isOwnerSubscriber = await _unitOfWork.SubscriberRepository
            .CheckIsUserSubscriber(entityOwnerId, userId);
        
        if (!isOwnerSubscriber)
        {
            throw new BadRequestException("Comments is disable");
        }
    }
}
