using System.Text.Json.Serialization;

namespace NftMetadataLoader.Common;

public class MetadataDto
{
    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    [JsonPropertyName("animation_url")]
    public string AnimationUrl { get; set; } = string.Empty;
}
