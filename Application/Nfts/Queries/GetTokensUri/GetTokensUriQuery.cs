using Application.Common.DTO.Nfts;
using MediatR;

namespace Application.Nfts.Queries.GetTokensUri;

public class GetTokensUriQuery : IRequest<TokenUriDto[]>
{
    public string[] Links { get; set; } = Array.Empty<string>();
}
