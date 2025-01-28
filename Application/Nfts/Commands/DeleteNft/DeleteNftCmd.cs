using MediatR;

namespace Application.Nfts.Commands.DeleteNft;

public class DeleteNftCmd : IRequest
{
    public int UserId { get; set; }
    public int NftId { get; set; }
}
