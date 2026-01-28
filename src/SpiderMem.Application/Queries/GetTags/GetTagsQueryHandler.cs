using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.DTOs;
using SpiderMem.Persistence.Data;

namespace SpiderMem.Application.Queries.GetTags;

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, Result<List<TagDto>>>{

    private readonly AppDbContext _context;

    public GetTagsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TagDto>>> Handle(GetTagsQuery request, CancellationToken cancellationToken){
        var tags = await _context.Tags
            .Select(m => new TagDto(
                m.Id,
                m.Name
            ))
            .ToListAsync(cancellationToken);

        return Result<List<TagDto>>.Success(tags);
    }
}
