using Application.Common.DTO;
using Application.Common.DTO.Nfts;
using Application.Common.Enums;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Nfts.Queries.GetUserNftList;

public class GetUserNftListHandler : IRequestHandler<GetUserNftListQuery, PagedList<NftDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserNftListHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<NftDto>> Handle(GetUserNftListQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.NftRepository.GetUserNftList(request.UserId, request.PageIndex, request.PageSize);
        var mappedItems = _mapper.Map<NftDto[]>(list.Items);

        foreach (var item in mappedItems)
        {
            item.LikeCount = await _unitOfWork.LikeRepository.GetLikeCount(item.Id, ContentType.Nft);
            item.IsLiked = await _unitOfWork.LikeRepository.CheckExist(request.UserId, item.Id, ContentType.Nft);
            item.CommentCount = await _unitOfWork.CommentRepository.GetCommentCount(item.Id, ContentType.Nft);
        }
        
        return new PagedList<NftDto>
        {
            Items = mappedItems,
            PageIndex = list.PageIndex,
            PageSize = list.PageSize,
            TotalCount = list.TotalCount,
            TotalPages = list.TotalPages
        };
    }
}
