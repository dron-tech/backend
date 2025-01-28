namespace Moralis.Common.Responses;

public class NftByWalletResponse
{
    public int Total { get; set; }
    public int Page { get; set; }
    public string Cursor { get; set; } = string.Empty;
    public NftResponse[] Result { get; set; } = Array.Empty<NftResponse>();
}
