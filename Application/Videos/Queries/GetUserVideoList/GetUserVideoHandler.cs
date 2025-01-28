using Application.Common.DTO;
using Application.Common.DTO.Videos;
using Application.Common.Enums;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Videos.Queries.GetUserVideoList;

public class GetUserVideoHandler : IRequestHandler<GetUserVideoListQuery, PagedList<VideoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserVideoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<VideoDto>> Handle(GetUserVideoListQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.VideoRepository.GetUserVideoList(request.UserId, request.PageIndex, request.PageSize);
        var mappedItems = _mapper.Map<VideoDto[]>(list.Items);

        foreach (var item in mappedItems)
        {
            item.LikeCount = await _unitOfWork.LikeRepository.GetLikeCount(item.Id, ContentType.Video);
            item.CommentCount = await _unitOfWork.CommentRepository.GetCommentCount(item.Id, ContentType.Video);
            item.IsLiked = await _unitOfWork.LikeRepository.CheckExist(request.UserId, item.Id, ContentType.Video);
        }
        
        return new PagedList<VideoDto>
        {
            Items = mappedItems,
            PageIndex = list.PageIndex,
            PageSize = list.PageSize,
            TotalCount = list.TotalCount,
            TotalPages = list.TotalPages
        };
    }
}
