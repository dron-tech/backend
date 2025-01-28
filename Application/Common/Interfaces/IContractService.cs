namespace Application.Common.Interfaces;

public interface IContractService
{
    public Task<string?> GetTokenUri(string contractAddress, int tokenId);
    public Task<string?> GetContractName(string contractAddress);
}
