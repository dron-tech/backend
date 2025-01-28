using System.Net.Http.Json;
using Application.Common.DTO.Nfts;
using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using NftMetadataLoader.Common;

namespace NftMetadataLoader;

public class NftMetadataLoaderService : INftMetadataLoaderService
{
    private readonly HttpClient _client;
    private readonly ILogger<NftMetadataLoaderService> _logger;
    private const string IpfsProtocolName = "ipfs";
    private const string IpfsGateway = "https://gateway.ipfs.io/ipfs/";
    private const string SafariUserAgent =
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0 Safari/605.1.15";

    public NftMetadataLoaderService(HttpClient client, ILogger<NftMetadataLoaderService> logger)
    {
        _client = client;
        _logger = logger;
        _client.DefaultRequestHeaders.Add("User-Agent", SafariUserAgent);
    }

    public async Task<NftMetadataDto> GetMetadata(string link)
    {
        try
        {
            if (IsIpfsUri(link))
            {
                link = GenerateHttpsLinkFromIpfs(link);
            }

            var response = await _client.GetAsync(link);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<MetadataDto>();
            if (content is null)
            {
                throw new Exception($"Empty response body from ipfs - {response}");
            }

            var image = content.Image is "" ? content.ImageUrl : content.Image;

            if (IsIpfsUri(image))
            {
                image = GenerateHttpsLinkFromIpfs(image);
            }

            var result = new NftMetadataDto
            {
                Image = image,
                AnimationUrl = content.AnimationUrl
            };

            return result;
        }
        catch (Exception e)
        {
            _logger.LogWarning("Error near loading metadata {E}", e);
            return new NftMetadataDto
            {
                Image = ""
            };
        }
    }

    private static bool IsIpfsUri(string uri)
    {
        return uri.Split(":")[0] is IpfsProtocolName;
    }

    private static string GenerateHttpsLinkFromIpfs(string link)
    {
        var ipfsHash = link.Split("//")[1];
        return $"{IpfsGateway}{ipfsHash}";
    }
}
