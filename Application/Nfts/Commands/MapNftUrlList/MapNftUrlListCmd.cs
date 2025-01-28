using Application.Common.DTO.Nfts;
using MediatR;

namespace Application.Nfts.Commands.MapNftUrlList;

public class MapNftUrlListCmd : IRequest<MapNftUrlListResultDto>
{
    public int UserId { get; set; }
    public string[] Urls { get; set; } = Array.Empty<string>();
}
