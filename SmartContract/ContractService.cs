using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using SmartContract.Common;

namespace SmartContract;

public class ContractService : IContractService
{
    private readonly ContractCfg _cfg;
    private readonly ILogger<ContractService> _logger;

    public ContractService(IOptions<ContractCfg> opts, ILogger<ContractService> logger)
    {
        _logger = logger;
        _cfg = opts.Value;
    }

    public async Task<string?> GetTokenUri(string contractAddress, int tokenId)
    {
        try
        {
            var web3 = new Web3(_cfg.HttpNode);
            var service = web3.Eth.ERC20.GetContractService(contractAddress);
            var input = new TokenUriFunction
            {
                Id = tokenId
            };

            return await service.ContractHandler.GetFunction<TokenUriFunction>().CallAsync<string>(input);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Error near getting token uri from contract {E}", e);
            return null;
        }
    }

    public async Task<string?> GetContractName(string contractAddress)
    {
        try
        {
            var web3 = new Web3(_cfg.HttpNode);
            var service = web3.Eth.ERC20.GetContractService(contractAddress);
            var input = new NameContractFunction();

            return await service.ContractHandler.GetFunction<NameContractFunction>().CallAsync<string>(input);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Error near contract name from contract {E}", e);
            return null;
        }
    }
}
