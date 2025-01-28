namespace Application.Common.DTO.Nfts;

public class UserErc20Nft
{
    public string TokenAddress { get; set; } = string.Empty;
    public string TokenId { get; set; } = string.Empty;
    public bool PossibleSpam { get; set; }
}
