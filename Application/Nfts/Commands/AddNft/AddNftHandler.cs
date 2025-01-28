using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using MediatR;

namespace Application.Nfts.Commands.AddNft;

public class AddNftHandler : IRequestHandler<AddNftCmd, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractService _contractService;
    private readonly INftMetadataLoaderService _nftMetadataService;

    public AddNftHandler(IUnitOfWork unitOfWork, IContractService contractService,
        INftMetadataLoaderService nftMetadataService)
    {
        _unitOfWork = unitOfWork;
        _contractService = contractService;
        _nftMetadataService = nftMetadataService;
    }

    public async Task<int> Handle(AddNftCmd request, CancellationToken cancellationToken)
    {
        var contractAddress = NftUrlUtil.GetContractAddress(request.Url);
        var tokenId = NftUrlUtil.GetTokenId(request.Url);

        if (contractAddress is not {} || tokenId is not {} id)
        {
            throw new BadRequestException("Invalid URL");
        }

        var tokenUri = await _contractService.GetTokenUri(contractAddress, id);
        if (tokenUri is null)
        {
            throw new BadRequestException("Invalid URL");
        }

        var contractName = await _contractService.GetContractName(contractAddress);
        if (contractName is null)
        {
            throw new BadRequestException("Invalid URL");
        }

        var metadata = await _nftMetadataService.GetMetadata(tokenUri);
        if (metadata.Image is "")
        {
            throw new BadRequestException("Unsupported token protocol");
        }

        var isNftExist = await _unitOfWork.NftRepository.ExistUserNftWithSameUrl(metadata.Image, request.UserId);
        if (isNftExist)
        {
            throw new BadRequestException("Provided NFT already added");
        }
        
        var user = await _unitOfWork.UserRepository.GetByIdOrFail(request.UserId);
        var opts = new PublishOptions
        {
            CommentType = request.CommentType,
            LikeStyle = request.LikeStyle
        };
        
        var nft = new Nft
        {
            Desc = request.Desc,
            Url = metadata.Image,
            AnimationUrl = metadata.AnimationUrl,
            Name = $"{contractName}#{tokenId}",
            ExplorerUrl = request.Url,
            User = user,
            PublishOptions = opts
        };

        await _unitOfWork.NftRepository.Insert(nft);
        await _unitOfWork.CommitAsync(cancellationToken);

        return nft.Id;
    }


    
    
}
