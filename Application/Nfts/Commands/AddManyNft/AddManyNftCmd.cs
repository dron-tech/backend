using Application.Common.DTO.Nfts;
using MediatR;

namespace Application.Nfts.Commands.AddManyNft;

public class AddManyNftCmd : IRequest<AddNftsResultDto>
{
    public int UserId { get; set; }
    public AddNftDto[] Nfts { get; set; } = Array.Empty<AddNftDto>();
}
