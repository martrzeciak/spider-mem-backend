using Microsoft.AspNetCore.Mvc;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Queries.GetMemes;
using SpiderMem.Application.Queries.GetMemeDetails;
using SpiderMem.Application.Queries.GetMemesByTag;
using SpiderMem.Application.Commands.CreateMeme;
using SpiderMem.Application.Commands.ToggleLike;
using Microsoft.AspNetCore.Authorization;
using SpiderMem.Application.Interfaces;

namespace SpiderMem.API.Controllers;

public class MemeController(IImageService imageService) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedList<MemeDto>>> GetMemes([FromQuery] MemeParams memeParams){
        return HandlePagedResult(
            await Mediator.Send(
                new GetMemesQuery { MemeParams = memeParams }
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
    public async Task<ActionResult<PagedList<MemeDto>>> GetMemesByTag(Guid tagId, [FromQuery] MemeParams memeParams){
        return HandlePagedResult(await Mediator.Send(
            new GetMemesByTagQuery{
                TagId = tagId,
                MemeParams = memeParams
            }
        ));
    }

    [Authorize]
    [HttpPost("{Id:guid}/toggle")]
    public async Task<ActionResult<int>> ToggleLike(Guid id)
    {
        return HandleResult(await Mediator.Send(new ToggleLikeCommand(id)));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<MemeDto>> CreateMeme(CreateMemeCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }


    [HttpPost("create")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] AddMemeDto dto)
    {
        var uploadResult = await imageService.AddImageAsync(dto.ImageUrl, "memes");
        if (uploadResult.Error != null)
            return BadRequest(uploadResult.Error.Message);

        var command = new CreateMemeCommand
        {
            Title = dto.Title,
            ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
            TagIds = dto.Tags1 ?? new List<Guid>()
        };

        var result = await Mediator.Send(command);
        if (!result.IsSuccess) return BadRequest(result.Error);

        return Ok(result.Value);
    }
}