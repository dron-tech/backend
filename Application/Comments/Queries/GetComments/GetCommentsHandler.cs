using Application.Common.DTO;
using Application.Common.DTO.Comments;
using Application.Common.DTO.Profiles;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common.Enums;
using Domain.Entities;
using MediatR;
using Profile = Domain.Entities.Profile;

namespace Application.Comments.Queries.GetComments;

public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, PagedList<CommentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCommentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<CommentDto>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        switch (request.Type)
        {
            case ContentType.Video:
                await EnsureHaveAccessToVideo(request.EntityId, request.UserId);
                break;
            case ContentType.Nft:
                await EnsureHaveAccessToNft(request.EntityId, request.UserId);
                break;
            case ContentType.Comment:
                await EnsureHaveAccessToComment(request.EntityId, request.UserId);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(request.Type));
        }

        var list = await _unitOfWork.CommentRepository.GetPagedList(request.EntityId, request.Type,
            request.PageIndex, request.PageSize);

        var mappedItems = new List<CommentDto>();
        foreach (var item in list.Items)
        {
            var profile = await _unitOfWork.ProfileRepository.GetUserProfile(item.UserId) ?? new Profile();

            var dto = new CommentDto
            {
                Id = item.Id,
                Value = item.Value,
                UserId = item.UserId,
                Profile = _mapper.Map<ProfileDto>(profile),
                Login = await _unitOfWork.UserRepository.GetUserLoginOrFail(item.UserId),
                LikeCount = await _unitOfWork.LikeRepository.GetLikeCount(item.Id, ContentType.Comment),
                IsLiked = await _unitOfWork.LikeRepository.CheckExist(request.UserId, item.Id, ContentType.Comment),
                CreatedAt = item.CreatedAt,
                CommentCount = await _unitOfWork.CommentRepository.GetCommentCount(item.Id, ContentType.Comment)
            };
            
            mappedItems.Add(dto);
        }

        return new PagedList<CommentDto>
        {
            Items = mappedItems.ToArray(),
            PageIndex = list.PageIndex,
            PageSize = list.PageSize,
            TotalCount = list.TotalCount,
            TotalPages = list.TotalPages
        };
    }

    private async Task EnsureHaveAccessToComment(int commentId, int userId)
    {
        var comment = await _unitOfWork.CommentRepository.GetById(commentId);
        if (comment is null)
        {
            throw new NotFoundException("Comment not found");
        }
        
        if (comment.NftId is { } nftId)
        {
            await EnsureHaveAccessToNft(nftId, userId);
        } 
        else if (comment.VideoId is { } videoId)
        {
            await EnsureHaveAccessToVideo(videoId, userId);
        }
        else
        {
            throw new NotFoundException("Comment not found");    
        }
    }

    private async Task EnsureHaveAccessToNft(int nftId, int userId)
    {
        var nft = await _unitOfWork.NftRepository.GetById(nftId);
        if (nft is null)
        {
            throw new NotFoundException("Nft not found");
        }

        var opts = await _unitOfWork.PublishOptionsRepository.GetByIdOrFail(nft.PublishOptionsId);
        await EnsureHaveAccessByPublishOpts(opts, userId, nft.UserId);
    }

    private async Task EnsureHaveAccessToVideo(int videoId, int userId)
    {
        var video = await _unitOfWork.VideoRepository.GetById(videoId);
        if (video is null)
        {
            throw new NotFoundException("Video not found");
        }

        var opts = await _unitOfWork.PublishOptionsRepository.GetByIdOrFail(video.PublishOptionsId);
        await EnsureHaveAccessByPublishOpts(opts, userId, video.UserId);
    }

    private async Task EnsureHaveAccessByPublishOpts(PublishOptions opts, int userId, int entityOwnerId)
    {
        switch (opts.CommentType)
        {
            case CommentType.AllowEveryone:
                return;
            case CommentType.Disable:
                throw new BadRequestException("Comments is disable");
            case CommentType.SubscriptionsOnly:
                break;
            default:
                throw new ArgumentException($"Comment type out of range {opts.CommentType}");
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
