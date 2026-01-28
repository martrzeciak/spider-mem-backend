using Microsoft.AspNetCore.Mvc;
using SpiderMem.Application.Queries.GetUserDetails;
using SpiderMem.Application.Queries.GetCurrentUser;

namespace SpiderMem.API.Controllers;

public class UserController : BaseApiController
{
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        return HandleResult(await Mediator.Send(new GetCurrentUserQuery()));
    }

    [HttpGet("{Id:guid}/details")]
    public async Task<IActionResult> GetUserDetails()
    {
        return HandleResult(await Mediator.Send(new GetUserDetailsQuery()));
    }

}