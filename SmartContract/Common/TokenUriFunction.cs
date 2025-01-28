using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartContract.Common;

[Function("tokenURI", "string")]
public class TokenUriFunction
{
    [Parameter("uint256", "id")]
    public int Id { get; set; }  
}
