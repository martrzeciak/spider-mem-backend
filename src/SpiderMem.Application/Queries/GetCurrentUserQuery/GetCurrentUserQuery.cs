using MediatR;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<Result<UserDto>>;