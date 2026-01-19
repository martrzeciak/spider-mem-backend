using SpiderMem.Application.DTOs;
using MediatR;

namespace SpiderMem.Application.Queries.GetMemes;

public class GetMemesQuery : IRequest<Result<List<MemeDto>>>
{
    public int Page { get; set; } = 1;
}
