using Application.Common.DTO.Nfts;

namespace Application.Common.Interfaces;

public interface INftMetadataLoaderService
{
    public Task<NftMetadataDto> GetMetadata(string link);
}
