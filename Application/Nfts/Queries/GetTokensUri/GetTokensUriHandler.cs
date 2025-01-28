using Application.Common.DTO.Nfts;
using Application.Common.Interfaces;
using Application.Common.Utils;
using MediatR;

namespace Application.Nfts.Queries.GetTokensUri;

public class GetTokensUriHandler : IRequestHandler<GetTokensUriQuery, TokenUriDto[]>
{
    private readonly IContractService _contractService;

    public GetTokensUriHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public async Task<TokenUriDto[]> Handle(GetTokensUriQuery request, CancellationToken cancellationToken)
    {
        var result = new List<TokenUriDto>();

        foreach (var item in request.Links)
        {
            var contractAddress = NftUrlUtil.GetContractAddress(item);
            var tokenId = NftUrlUtil.GetTokenId(item);
    
            if (contractAddress is not {} || tokenId is not {} id)
            {
                continue;
            }
    
            var tokenUri = await _contractService.GetTokenUri(contractAddress, id);
            if (tokenUri is not null)
            {
                result.Add(new TokenUriDto
                {
                    Link = tokenUri
                });
            }
        }

        return result.ToArray();
    }
}
