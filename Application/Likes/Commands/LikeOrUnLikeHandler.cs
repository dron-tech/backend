using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Likes.Commands;

public class LikeOrUnLikeHandler : IRequestHandler<LikeOrUnLikeCmd>
{
    private readonly IUnitOfWork _unitOfWork;

    public LikeOrUnLikeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(LikeOrUnLikeCmd request, CancellationToken cancellationToken)
    {
        await EnsureEntityExist(request);

        var like = await _unitOfWork.LikeRepository.GetUserLike(request.UserId, request.Id, request.Type);
        if (like is null)
        {
            var newLike = new Like
            {
                UserId = request.UserId
            };

            switch (request.Type)
            {
                case ContentType.Comment:
                    newLike.CommentId = request.Id;
                    break;
                
                case ContentType.Nft:
                    newLike.NftId = request.Id;
                    break;
                
                case ContentType.Video:
                    newLike.VideoId = request.Id;
                    break;
                
                default:
                    throw new ArgumentException($"Unsupported like type {request.Type}");
            }

            await _unitOfWork.LikeRepository.Insert(newLike);
        }
        else
        {
            _unitOfWork.LikeRepository.Remove(like);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task EnsureEntityExist(LikeOrUnLikeCmd cmd)
    {
        switch (cmd.Type)
        {
            case ContentType.Video:
            {
                var isExist = await _unitOfWork.VideoRepository.IsVideoExist(cmd.Id);
                if (!isExist)
                {
                    throw new NotFoundException("Video not found");
                }

                return;
            }
            case ContentType.Nft:
            {
                var nft = await _unitOfWork.NftRepository.GetById(cmd.Id);
                if (nft is null)
                {
                    throw new NotFoundException("Nft not found");
                }

                break;
            }
            case ContentType.Comment:
            {
                var comment = await _unitOfWork.CommentRepository.GetById(cmd.Id);
                if (comment is null)
                {
                    throw new NotFoundException("Comment not found");
                }

                break;
            }
            default:
                throw new ArgumentException($"Unsupported like type, {cmd.Type}");
        }
    }
}
