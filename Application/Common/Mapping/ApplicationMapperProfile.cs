using Application.Common.DTO.Nfts;
using Application.Common.DTO.Profiles;
using Application.Common.DTO.PublishOptions;
using Application.Common.DTO.Users;
using Application.Common.DTO.Videos;
using Domain.Common.Enums;
using Domain.Entities;

namespace Application.Common.Mapping;

public class ApplicationMapperProfile : AutoMapper.Profile
{
    public ApplicationMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<PublishOptions, PublishOptionsDto>();
        CreateMap<Profile, ProfileDto>()
            .ForMember(d => d.Status,
                opt =>
                    opt.MapFrom(s => s.Status != null ?
                        UserStatusMapper.MapToStr((UserStatus)s.Status) : null));
        
        CreateMap<Video, VideoDto>()
            .ForMember(d => d.Options,
                opt => 
                    opt.MapFrom(s => s.PublishOptions));
        
        CreateMap<Nft, NftDto>()
            .ForMember(d => d.Options,
                opt => 
                    opt.MapFrom(s => s.PublishOptions));
        
    }
}
