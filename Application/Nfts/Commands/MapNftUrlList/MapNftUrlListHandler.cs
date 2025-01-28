using Application.Common.DTO.Nfts;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Nfts.Commands.MapNftUrlList;

public class MapNftUrlListHandler : IRequestHandler<MapNftUrlListCmd, MapNftUrlListResultDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public MapNftUrlListHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MapNftUrlListResultDto> Handle(MapNftUrlListCmd request, CancellationToken cancellationToken)
    {
        var nfts = await _unitOfWork.NftRepository.GetExistNftUrls(request.UserId, request.Urls);
        var result = new MapNftUrlListResultDto();

        foreach (var item in request.Urls)
        {
            var existNft = nfts.FirstOrDefault(x => x.ExplorerUrl == item);
            if (existNft is not null)
            {
                result.AlreadyAdded.Add(new AlreadyAddedNft
                {
                    Id = existNft.Id,
                    ExplorerUrl = existNft.ExplorerUrl
                });
            }
            else
            {
                result.NotAdded.Add(item);
            }
        }

        return result;
    }
}
