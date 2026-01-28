using MediatR;
using SpiderMem.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace SpiderMem.Application.Commands.CreateTag;

public class CreateTagCommand : IRequest<Result<TagDto>>
{
    public string Name { get; init; } = null!;
}
