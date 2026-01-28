using MediatR;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Queries.GetMemesByTag;

public class GetMemesByTagQuery : IRequest<Result<PagedList<MemeDto>>>
{
    public Guid TagId { get; set; }
    
    public required MemeParams MemeParams{ get; set; }
}
