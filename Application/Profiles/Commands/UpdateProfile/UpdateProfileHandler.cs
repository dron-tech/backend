using Application.Common.DTO.Users;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using AutoMapper;
using MediatR;
using Profile = Domain.Entities.Profile;

namespace Application.Profiles.Commands.UpdateProfile;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCmd, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsService _awsService;
    private const int ChangeLoginCooldownInDays = 14;

    public UpdateProfileHandler(IUnitOfWork unitOfWork, IMapper mapper, IAwsService awsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsService = awsService;
    }

    public async Task<UserDto> Handle(UpdateProfileCmd request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdOrFail(request.UserId);
        var profile = await _unitOfWork.ProfileRepository.GetUserProfile(request.UserId);

        if (profile is null)
        {
            profile = new Profile
            {
                User = user
            };
            await _unitOfWork.ProfileRepository.Insert(profile);
        }

        if (request.Login is not null)
        {
            if (await _unitOfWork.UserRepository.ExistByLogin(request.Login))
            {
                throw new BadRequestException("Login already using");
            }
            
            if (DateTime.UtcNow < user.LastLoginUpdate.AddDays(ChangeLoginCooldownInDays))
            {
                throw new BadRequestException("Change username no more than once every 2 weeks");
            }
            
            user.Login = request.Login;
        }
        
        if (request.Name is not null)
        {
            profile.Name = request.Name;
        }

        if (request.Desc is not null)
        {
            profile.Desc = request.Desc;
        }

        if (request.Link is not null)
        {
            profile.Link = request.Link;
        }

        if (request.Status is not null)
        {
            profile.Status = request.Status;
        }

        if (request.Avatar is not null)
        {
            if (profile.Avatar is not null)
            {
                await _awsService.DeleteFile(profile.Avatar);
            }

            profile.Avatar = FileHelperUtil.GenerateFileName(request.Avatar.ContentType);
            await _awsService.UploadFile(profile.Avatar, request.Avatar.OpenReadStream(), request.Avatar.ContentType);
        }
        
        if (request.Cover is not null)
        {
            if (profile.Cover is not null)
            {
                await _awsService.DeleteFile(profile.Cover);
            }
            
            profile.Cover = FileHelperUtil.GenerateFileName(request.Cover.ContentType);
            await _awsService.UploadFile(profile.Cover, request.Cover.OpenReadStream(), request.Cover.ContentType);

        }

        await _unitOfWork.CommitAsync(cancellationToken);
        
        user.Profile = profile;
        
        var result = _mapper.Map<UserDto>(user);
        result.PublicationStat = await GetPubStat(user.Id);

        return result;
    }
    
    private async Task<PublicationStat> GetPubStat(int userId)
    {
        return new PublicationStat
        {
            VideoCount = await _unitOfWork.VideoRepository.GetUserVideoCount(userId),
            NftCount = await _unitOfWork.NftRepository.GetUserNftCount(userId)
        };
    }
}
