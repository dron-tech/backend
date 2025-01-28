using System.Diagnostics;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using FFMpegCore;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Videos.Commands.UploadVideo;

public class UploadVideoHandler : IRequestHandler<UploadVideoCmd, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly IAwsService _awsService;
    private readonly string _rootPath;
    private const int MaxVideoDurationInMin = 3;
    private const string GifContentType = "image/gif";
    private readonly ILogger<UploadVideoHandler> _logger;

    public UploadVideoHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService, IHostingEnvironment env,
        IAwsService awsService, ILogger<UploadVideoHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _awsService = awsService;
        _logger = logger;
        _rootPath = env.WebRootPath;
    }

    public async Task<int> Handle(UploadVideoCmd request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdOrFail(request.UserId);
        var opts = new PublishOptions
        {
            CommentType = request.CommentType,
            LikeStyle = request.LikeStyle,
        };
        
        var newVideo = new Video
        {
            Desc = request.Desc,
            User = user,
            Location = request.Location,
            PublishOptions = opts
        };
        
        try
        {
            if (request.Cover is not null)
            {
                newVideo.Cover = FileHelperUtil.GenerateFileName(request.Cover.ContentType);
                await _awsService.UploadFile(newVideo.Cover, request.Cover.OpenReadStream(), request.Cover.ContentType);
            }
        
            var videoFileName = FileHelperUtil.GenerateFileName(request.Video!.ContentType);
            await _fileStorageService.Save(request.Video, videoFileName);
            
            newVideo.FileNameFull = videoFileName;
            
            var dur = await GetVideoDuration(Path.Combine(_rootPath, videoFileName));
            if (dur > TimeSpan.FromMinutes(MaxVideoDurationInMin))
            {
                throw new BadRequestException($"Max video duration can be {MaxVideoDurationInMin} min");
            }

            newVideo.Duration = dur;
            
            var gif = await GenerateGifAndSave(videoFileName);
            newVideo.FileNameShort = gif;

            await _awsService.UploadFile(newVideo.FileNameFull, request.Video.OpenReadStream(),
                request.Video.ContentType);

            await _awsService.UploadFile(newVideo.FileNameShort, Path.Combine(_rootPath, newVideo.FileNameShort),
                GifContentType);

            DeleteAllFiles(newVideo);
            await _unitOfWork.VideoRepository.Insert(newVideo);
            await _unitOfWork.CommitAsync(cancellationToken);
            
            return newVideo.Id;
        }
        catch (Exception)
        {
            DeleteAllFiles(newVideo, true);
            throw;
        }
    }

    private static async Task<TimeSpan> GetVideoDuration(string videoPath)
    {
        var result = await FFProbe.AnalyseAsync(videoPath);
        return result.Duration;
    }

    private async Task<string> GenerateGifAndSave(string videoName)
    {
        var videoFullPath = Path.Combine(_rootPath, videoName);

        var gifFileName = FileHelperUtil.GenerateFileName(GifContentType);
        var gifFullPath = Path.Combine(_rootPath, gifFileName);

        var result = await TryGenerateGif(videoFullPath, gifFullPath);
        
        if (!result)
        {
            throw new Exception("Error while converting video to gif");
        }

        return gifFileName;
    }

    private async Task<bool> TryGenerateGif(string videoPath, string gifPath)
    {
        try
        {
            return await FFMpeg.GifSnapshotAsync(
                videoPath,
                gifPath,
                null,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2));
        }
        catch (Exception e)
        {
            _logger.LogWarning("Error near creating gif\n{E}", e);
        }
        
        var proc = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = "ffmpeg",
                Arguments = $"-t 3 -i {videoPath} -y {gifPath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        proc.Start();
        await proc.WaitForExitAsync();

        return proc.ExitCode == 0;
    }

    private void DeleteAllFiles(Video video, bool includeAws = false)
    {
        if (video.Cover is not null)
        {
            _fileStorageService.Delete(video.Cover);
            if (includeAws)
            {
                _awsService.DeleteFile(video.Cover);
            }
        }

        _fileStorageService.Delete(video.FileNameFull);
        _fileStorageService.Delete(video.FileNameShort);

        if (includeAws)
        {
            _awsService.DeleteFile(video.FileNameFull);
            _awsService.DeleteFile(video.FileNameShort);
        }
    }
}
