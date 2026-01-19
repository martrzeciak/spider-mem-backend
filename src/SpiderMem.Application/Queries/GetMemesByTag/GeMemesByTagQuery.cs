using MediatR;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Queries.GetMemesByTag;

public class GetMemesByTagQuery : IRequest<Result<List<MemeDto>>>
{
    public Guid TagId { get; set; }
    public int Page { get; set; } = 1;
}
