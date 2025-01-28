using Application.Common.DTO.Nfts;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;

namespace Application.Nfts.Commands.AddManyNft;

public class AddManyNftHandler : IRequestHandler<AddManyNftCmd, AddNftsResultDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractService _contractService;
    private readonly INftMetadataLoaderService _nftMetadataService;

    public AddManyNftHandler(IUnitOfWork unitOfWork, IContractService contractService, INftMetadataLoaderService nftMetadataService)
    {
        _unitOfWork = unitOfWork;
        _contractService = contractService;
        _nftMetadataService = nftMetadataService;
    }

    public async Task<AddNftsResultDto> Handle(AddManyNftCmd request, CancellationToken cancellationToken)
    {
        var fails = new List<AddNftFailDto>();

        foreach (var item in request.Nfts)
        {
            var contractAddress = NftUrlUtil.GetContractAddress(item.Url);
            var tokenId = NftUrlUtil.GetTokenId(item.Url);
    
            if (contractAddress is not {} || tokenId is not {} id)
            {
                fails.Add(GenerateFailDto(item, "Invalid URL"));
                continue;
            }
    
            var tokenUri = await _contractService.GetTokenUri(contractAddress, id);
            if (tokenUri is null)
            {
                fails.Add(GenerateFailDto(item, "Invalid URL"));
                continue;
            }
    
            var contractName = await _contractService.GetContractName(contractAddress);
            if (contractName is null)
            {
                fails.Add(GenerateFailDto(item, "Invalid URL"));
                continue;
            }
    
            var metadata = await _nftMetadataService.GetMetadata(tokenUri);
            if (metadata.Image is "")
            {
                fails.Add(GenerateFailDto(item,"Unsupported token protocol"));
                continue;
            }

            var isNftExist = await _unitOfWork.NftRepository.ExistUserNftWithSameUrl(metadata.Image, request.UserId);
            if (isNftExist)
            {
                fails.Add(GenerateFailDto(item,"Provided NFT already added"));
                continue;
            }
            
            var user = await _unitOfWork.UserRepository.GetByIdOrFail(request.UserId);
            var opts = new PublishOptions
            {
                CommentType = item.CommentType,
                LikeStyle = item.LikeStyle
            };
            
            var nft = new Nft
            {
                Desc = item.Desc,
                Url = metadata.Image,
                AnimationUrl = metadata.AnimationUrl,
                Name = $"{contractName}#{tokenId}",
                ExplorerUrl = item.Url,
                User = user,
                PublishOptions = opts
            };
    
            await _unitOfWork.NftRepository.Insert(nft);
        }
        
        await _unitOfWork.CommitAsync(cancellationToken);
        return new AddNftsResultDto
        {
            Fails = fails.ToArray()
        };
    }

    private static AddNftFailDto GenerateFailDto(AddNftDto dto, string reason)
    {
        return new AddNftFailDto
        {
            Desc = dto.Desc,
            Url = dto.Url,
            CommentType = dto.CommentType,
            LikeStyle = dto.LikeStyle,
            Reason = reason
        };
    }
}
