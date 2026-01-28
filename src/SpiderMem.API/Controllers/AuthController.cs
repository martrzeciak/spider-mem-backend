using Microsoft.AspNetCore.Mvc;
using SpiderMem.Application.Commands.Auth;
using SpiderMem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

public class AuthController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var result = await Mediator.Send(new LoginCommand(loginDto));
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
