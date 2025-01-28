using Microsoft.AspNetCore.Mvc;

namespace WebApp.Common.DTO;

public class GetPagedListDto
{
    [FromQuery(Name = "size")]
    public int PageSize { get; set; }

    [FromQuery(Name = "index")]
    public int PageIndex { get; set; }
}
