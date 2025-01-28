using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Nfts.Commands.UpdateNft;

public class UpdateNftHandler : IRequestHandler<UpdateNftCmd>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNftHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateNftCmd request, CancellationToken cancellationToken)
    {
        var nft = await _unitOfWork.NftRepository.GetById(request.NftId);
        if (nft?.UserId != request.UserId)
        {
            throw new NotFoundException("Nft not found");
        }

        var opts = await _unitOfWork.PublishOptionsRepository.GetByIdOrFail(nft.PublishOptionsId);

        if (request.Desc is not null)
        {
            nft.Desc = request.Desc;
        }

        if (request.LikeStyle is {} likeStyle)
        {
            opts.LikeStyle = likeStyle;
        }

        if (request.CommentType is {} commentType)
        {
            opts.CommentType = commentType;
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
