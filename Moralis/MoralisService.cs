using System.Net.Http.Json;
using Application.Common.DTO.Nfts;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moralis.Common.Configs;
using Moralis.Common.Responses;

namespace Moralis;

public class MoralisService : IMoralisService
{
    private readonly HttpClient _httpClient;
    private readonly MoralisCfg _cfg;
    private readonly ILogger<MoralisService> _logger;
    
    private const string GetErc20TokensUrn = "/nft?chain=eth&format=decimal&disable_total=false&media_items=false";
    private const string MoralisApiUrl = "https://deep-index.moralis.io/api/v2/";
    private const string ApiKeyHeaderKey = "X-API-Key";

    public MoralisService(HttpClient httpClient, IOptions<MoralisCfg> opts, ILogger<MoralisService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cfg = opts.Value;
    }

    public async Task<UserErc20Nft[]> GetNftByWallet(string address)
    {
        var result = new List<UserErc20Nft>();
        string? cursor = null;
        do
        {
            var uri = GenerateGetErc20TokenUri(address);
            if (cursor is not null)
            {
                uri += $"&cursor={cursor}";
            }
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
            };
        
            request.Headers.Add(ApiKeyHeaderKey, _cfg.ApiKey);
            
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var txtContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Error near getting NFTs from moralis\n{E}", txtContent);
                throw new BadRequestException("Nft service not working");
            }

            var content = await response.Content.ReadFromJsonAsync<NftByWalletResponse>();
            if (content is null)
            {
                var txtContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Error near reading response from moralis\n{E}", txtContent);
                throw new BadRequestException("Nft service not working");
            }

            if (content.Result.Length == 0)
            {
                cursor = null;
                continue;
            }
            
            var erc20Tokens = content.Result.Where(x => x.ContractType == "ERC721");
            result.AddRange(erc20Tokens.Select(x => new UserErc20Nft
            {
                TokenAddress = x.TokenAddress,
                TokenId = x.TokenId,
                PossibleSpam = x.PossibleSpam
            }));

            cursor = content.Cursor;
            request.RequestUri = new Uri(GenerateGetErc20TokenUri(address) + $"?cursor={cursor}");

        } while (cursor is not null);

        return result.ToArray();
    }

    private static string GenerateGetErc20TokenUri(string address) => $"{MoralisApiUrl}{address}{GetErc20TokensUrn}";
}
