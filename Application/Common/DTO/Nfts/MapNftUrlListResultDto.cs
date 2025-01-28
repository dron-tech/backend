using System.Collections.ObjectModel;

namespace Application.Common.DTO.Nfts;

public class MapNftUrlListResultDto
{
    public Collection<AlreadyAddedNft> AlreadyAdded { get; set; } = new();
    public Collection<string> NotAdded { get; set; } = new();
}

public class AlreadyAddedNft
{
    public int Id { get; set; }
    public string ExplorerUrl { get; set; } = string.Empty;
}
