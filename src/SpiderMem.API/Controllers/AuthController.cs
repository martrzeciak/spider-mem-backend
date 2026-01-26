using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpiderMem.Application.Commands.Auth;
using SpiderMem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace SpiderMem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var result = await _mediator.Send(new LoginCommand(loginDto));
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
