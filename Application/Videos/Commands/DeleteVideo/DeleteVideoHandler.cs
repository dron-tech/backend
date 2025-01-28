using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Videos.Commands.DeleteVideo;

public class DeleteVideoHandler : IRequestHandler<DeleteVideoCmd>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsService _awsService;

    public DeleteVideoHandler(IUnitOfWork unitOfWork, IAwsService awsService)
    {
        _unitOfWork = unitOfWork;
        _awsService = awsService;
    }

    public async Task Handle(DeleteVideoCmd request, CancellationToken cancellationToken)
    {
        var video = await _unitOfWork.VideoRepository.GetUserVideoById(request.VideoId, request.UserId);
        if (video is null)
        {
            throw new NotFoundException("Video not found");
        }

        await DeleteVideoFiles(video);
        
        _unitOfWork.VideoRepository.Remove(video);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task DeleteVideoFiles(Video video)
    {
        if (video.Cover is not null)
        {
            await _awsService.DeleteFile(video.Cover);
        }
        
        await _awsService.DeleteFile(video.FileNameFull);
        await _awsService.DeleteFile(video.FileNameShort);
    }
}
