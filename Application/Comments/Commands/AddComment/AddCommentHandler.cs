using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Common.Enums;
using Domain.Entities;
using MediatR;

namespace Application.Comments.Commands.AddComment;

public class AddCommentHandler : IRequestHandler<AddCommentCmd, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCommentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddCommentCmd request, CancellationToken cancellationToken)
    {
        await EnsureCanAddComment(request.EntityId, request.UserId, request.IsVideo);
        var comment = new Comment
        {
            Value = request.Value,
            UserId = request.UserId,
        };

        if (request.IsVideo)
        {
            comment.VideoId = request.EntityId;
        }
        else
        {
            comment.NftId = request.EntityId;
        }

        await _unitOfWork.CommentRepository.Insert(comment);
        await _unitOfWork.CommitAsync(cancellationToken);

        return comment.Id;
    }

    private async Task EnsureCanAddComment(int entityId, int userId, bool isVideo)
    {
        int publishOptsId;
        int entityOwnerId;
        
        if (isVideo)
        {
            var video = await _unitOfWork.VideoRepository.GetById(entityId);
            if (video is null)
            {
                throw new NotFoundException("Video not found");
            }

            publishOptsId = video.PublishOptionsId;
            entityOwnerId = video.UserId;
        }
        else
        {
            var nft = await _unitOfWork.NftRepository.GetById(entityId);
            if (nft is null)
            {
                throw new NotFoundException("Nft not found");
            }

            publishOptsId = nft.PublishOptionsId;
            entityOwnerId = nft.UserId;
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
