using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpiderMem.API.Extensions;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Queries.GetMemes;
using SpiderMem.Application.Queries.GetMemeDetails;
using SpiderMem.Application.Queries.GetMemesByTag;
using SpiderMem.Application.Commands.CreateMeme;
using SpiderMem.Application.Commands.ToggleLike;
using SpiderMem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

[Route("api/[controller]")]
public class MemeController : BaseApiController
{
    private readonly IImageService _imageService;
    private readonly IMediator _mediator;

    public MemeController(IImageService imageService, IMediator mediator)
    {
        _imageService = imageService;
        _mediator = mediator;
    }


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

    [HttpPost("{Id:guid}/toggle")]
    public async Task<ActionResult<int>> ToggleLike(Guid id)
    {
        return HandleResult(await Mediator.Send(new ToggleLikeCommand(id)));
    }

    [HttpPost("create")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] AddMemeDto dto)
    {
        var uploadResult = await _imageService.AddImageAsync(dto.ImageUrl, "memes");
        if (uploadResult.Error != null)
            return BadRequest(uploadResult.Error.Message);

        var command = new CreateMemeCommand
        {
            Title = dto.Title,
            ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
            TagIds = dto.Tags1 ?? new List<Guid>()
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess) return BadRequest(result.Error);

        return Ok(result.Value);
    }
}