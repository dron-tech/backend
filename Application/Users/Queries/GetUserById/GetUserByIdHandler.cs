using Application.Common.DTO.Users;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Profile = Domain.Entities.Profile;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetById(request.UserId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        user.Profile = await _unitOfWork.ProfileRepository.GetUserProfile(request.UserId) ?? new Profile();
        
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
