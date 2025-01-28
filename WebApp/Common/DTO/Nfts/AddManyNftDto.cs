using Application.Common.DTO.Nfts;

namespace WebApp.Common.DTO.Nfts;

public class AddManyNftDto
{
    public AddNftDto[] Items { get; set; } = Array.Empty<AddNftDto>();
}
