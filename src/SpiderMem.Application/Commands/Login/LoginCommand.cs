using MediatR;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Commands.Auth;

public record LoginCommand(LoginDto LoginDto) : IRequest<Result<AuthResponseDto>>;
