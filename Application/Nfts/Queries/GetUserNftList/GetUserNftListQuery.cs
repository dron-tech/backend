using Application.Common.DTO;
using Application.Common.DTO.Nfts;
using MediatR;

namespace Application.Nfts.Queries.GetUserNftList;

public class GetUserNftListQuery : IRequest<PagedList<NftDto>>
{
    public int UserId { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
