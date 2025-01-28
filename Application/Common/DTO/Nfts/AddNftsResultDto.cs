namespace Application.Common.DTO.Nfts;

public class AddNftsResultDto
{
    public AddNftFailDto[] Fails { get; set; } = Array.Empty<AddNftFailDto>();
}
