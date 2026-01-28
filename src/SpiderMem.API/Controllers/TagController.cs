using Microsoft.AspNetCore.Mvc;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Queries.GetTags;
using SpiderMem.Application.Commands.CreateTag;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

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

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<MemeDto>> CreateTag(CreateTagCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }
}