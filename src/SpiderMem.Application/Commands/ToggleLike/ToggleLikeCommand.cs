using MediatR;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Commands.ToggleLike;

public record ToggleLikeCommand(Guid MemeId) : IRequest<Result<int>>;
