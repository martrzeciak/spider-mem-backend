using Microsoft.AspNetCore.Mvc;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Commands.CreateComment;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

[Authorize]
public class CommentController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }
}