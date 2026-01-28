using Microsoft.AspNetCore.Mvc;
using SpiderMem.Application.Commands.Auth;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Queries.GetCurrentUser;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseApiController
{
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        return HandleResult(await Mediator.Send(new GetCurrentUserQuery()));
    }

}