using Microsoft.AspNetCore.WebUtilities;

namespace Application.Common.Utils;

public static class NftUrlUtil
{
    private const string Authority = "etherscan.io";
    private const string ValidFirstSegment = "token/";
    private const string TokenIdKey = "a";
    private const int ValidSegmentLength = 3;
    
    public static bool Validate(string link)
    {
        try
        {
            var uri = new Uri(link);
            
            if (uri.Authority is not Authority)
            {
                return false;
            }

            if (uri.Segments.Length is not ValidSegmentLength)
            {
                return false;
            }

            if (uri.Segments[1] is not ValidFirstSegment)
            {
                return false;
            }

            var validTokenId = TryGetTokenId(uri, out _);
            return validTokenId && Nethereum.Util.AddressUtil.Current.IsValidEthereumAddressHexFormat(uri.Segments[2]);
        }
        catch
        {
            return false;
        }
    }

    public static string? GetContractAddress(string link)
    {
        var splitStr = link.Split("/");
        var lastPart = splitStr[^1];
        
        return lastPart.Split("?")[0];
    }

    public static int? GetTokenId(string link)
    {
        var result = TryGetTokenId(new Uri(link), out var id);
        return result ? id : null;
    }

    private static bool TryGetTokenId(Uri uri, out int id)
    {
        id = 0;
        var query = QueryHelpers.ParseQuery(uri.Query);
        var a = query.GetValueOrDefault(TokenIdKey);
        if (a.Count == 0)
        {
            return false;
        }

        var result = int.TryParse(a.First(), out var tokenId);
        if (!result)
        {
            return false;
        }

        id = tokenId;
        return true;
    } 
}
