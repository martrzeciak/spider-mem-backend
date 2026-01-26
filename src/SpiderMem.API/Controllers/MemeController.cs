using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpiderMem.API.Extensions;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Queries.GetMemes;
using SpiderMem.Application.Queries.GetMemeDetails;
using SpiderMem.Application.Queries.GetMemesByTag;
using SpiderMem.Application.Commands.CreateMeme;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

[Route("api/[controller]")]
public class MemeController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<MemeDto>>> GetMemes([FromQuery] int page = 1){
        return HandleResult(
            await Mediator.Send(
                new GetMemesQuery { Page = page }
            )
        );
    }
    
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemeDto>> GetMemeDetails(Guid id){
        return HandleResult(await Mediator.Send(
            new GetMemeDetailsQuery { MemeId = id }
        ));
    }

    [AllowAnonymous]
    [HttpGet("tag/{tagId:guid}")]
    public async Task<ActionResult<IEnumerable<MemeDto>>> GetMemesByTag(Guid tagId, [FromQuery] int page = 1){
        return HandleResult(await Mediator.Send(
            new GetMemesByTagQuery{
                TagId = tagId,
                Page = page
            }
        ));
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<MemeDto>> CreateMeme(CreateMemeCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }
}