using Microsoft.AspNetCore.Mvc;
using SpiderMem.API.Extensions;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Queries.GetTags;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

[Route("api/[controller]")]
public class TagController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<MemeDto>>> GetTags(){
        return HandleResult(
            await Mediator.Send(
                new GetTagsQuery{}
            )
        );
    }
}