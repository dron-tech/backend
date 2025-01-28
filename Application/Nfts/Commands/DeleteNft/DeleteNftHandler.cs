using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Nfts.Commands.DeleteNft;

public class DeleteNftHandler : IRequestHandler<DeleteNftCmd>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNftHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteNftCmd request, CancellationToken cancellationToken)
    {
        var nft = await _unitOfWork.NftRepository.GetById(request.NftId);
        if (nft?.UserId != request.UserId)
        {
            throw new NotFoundException("Nft not found");
        }

        _unitOfWork.NftRepository.Remove(nft);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
