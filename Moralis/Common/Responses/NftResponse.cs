using System.Text.Json.Serialization;

namespace Moralis.Common.Responses;

public class NftResponse
{
    [JsonPropertyName("token_address")]
    public string TokenAddress { get; set; } = string.Empty;

    [JsonPropertyName("token_id")]
    public string TokenId { get; set; }
    
    [JsonPropertyName("contract_type")]
    public string ContractType { get; set; } = string.Empty;
    
    [JsonPropertyName("possible_spam")]
    public bool PossibleSpam { get; set; }
}
