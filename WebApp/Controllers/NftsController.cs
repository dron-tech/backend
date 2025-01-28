using Application.Common.DTO;
using Application.Common.DTO.Nfts;
using Application.Nfts.Commands.AddManyNft;
using Application.Nfts.Commands.AddNft;
using Application.Nfts.Commands.DeleteNft;
using Application.Nfts.Commands.MapNftUrlList;
using Application.Nfts.Commands.UpdateNft;
using Application.Nfts.Queries.GetAllNftByWallet;
using Application.Nfts.Queries.GetTokensUri;
using Application.Nfts.Queries.GetUserNftList;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Attrs;
using WebApp.Common;
using WebApp.Common.DTO;
using WebApp.Common.DTO.Nfts;

namespace WebApp.Controllers;

[Authorize(Roles = UserRoles.User)]
[EnsureEmailIsConfirmAttr]
public class NftsController : BaseController
{
    public NftsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("Many")]
    public async Task<ApiResponse<AddNftsResultDto>> AddManyNft(AddManyNftDto dto)
    {
        var cmd = new AddManyNftCmd
        {
            UserId = GetUserId(),
            Nfts = dto.Items
        };

        return GetFormattedResponse(await Mediator.Send(cmd));
    }

    [HttpPost]
    public async Task<ApiResponse<int>> AddNft(AddNftDto dto)
    {
        var cmd = new AddNftCmd
        {
            UserId = GetUserId(),
            Desc = dto.Desc,
            Url = dto.Url,
            CommentType = dto.CommentType,
            LikeStyle = dto.LikeStyle
        };

        Response.StatusCode = 201;
        return GetFormattedResponse(await Mediator.Send(cmd), 201);
    }
    // ToDo rename to All/My
    [HttpGet("All")]
    public async Task<ApiResponse<PagedList<NftDto>>> GetMy([FromQuery] GetPagedListDto dto)
    {
        var qry = new GetUserNftListQuery
        {
            UserId = GetUserId(),
            PageIndex = dto.PageIndex,
            PageSize = dto.PageSize
        };

        return GetFormattedResponse(await Mediator.Send(qry));
    }

    [HttpGet("All/Wallet/{address}")]
    public async Task<ApiResponse<UserErc20Nft[]>> GetAllNftByWallet(string address)
    {
        var query = new GetAllNftByWalletQuery
        {
            Address = address
        };

        return GetFormattedResponse(await Mediator.Send(query));
    }

    [HttpPost("Uri")]
    public async Task<ApiResponse<TokenUriDto[]>> GetTokensUri(GetTokensUriQuery query)
    {
        return GetFormattedResponse(await Mediator.Send(query));
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteNft(int id)
    {
        var cmd = new DeleteNftCmd
        {
            UserId = GetUserId(),
            NftId = id
        };
        await Mediator.Send(cmd);
    }

    [HttpPut("{id:int}")]
    public async Task UpdateNft(int id, UpdateNftDto dto)
    {
        var cmd = new UpdateNftCmd
        {
            NftId = id,
            UserId = GetUserId(),
            Desc = dto.Desc,
            CommentType = dto.CommentType,
            LikeStyle = dto.LikeStyle,
        };

        await Mediator.Send(cmd);
    }

    [HttpPost("Unique")]
    public async Task<ApiResponse<MapNftUrlListResultDto>> GetUniqueNft(GetUniqueNftDto dto)
    {
        var cmd = new MapNftUrlListCmd
        {
            UserId = GetUserId(),
            Urls = dto.Urls
        };

        return GetFormattedResponse(await Mediator.Send(cmd));
    }
}

