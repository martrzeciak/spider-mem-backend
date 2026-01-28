using SpiderMem.Application.DTOs;
using MediatR;
using SpiderMem.Application.Common;

namespace SpiderMem.Application.Queries.GetMemes;

public class GetMemesQuery : IRequest<Result<PagedList<MemeDto>>>
{
    public required MemeParams MemeParams{ get; set; }
}
