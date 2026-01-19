using MediatR;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Queries.GetMemeDetails;

public class GetMemeDetailsQuery : IRequest<Result<MemeDto>>
{
    public Guid MemeId { get; set; }
}
