using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpiderMem.API.Extensions;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Commands.CreateComment;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentController : BaseApiController
{
    private readonly IMediator _mediator;

    public CommentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }
}