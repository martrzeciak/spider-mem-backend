using MediatR;
using SpiderMem.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace SpiderMem.Application.Commands.CreateMeme;

public class CreateMemeCommand : IRequest<Result<MemeDto>>
{
    public string Title { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public List<Guid> TagIds { get; init; } = new();
}
