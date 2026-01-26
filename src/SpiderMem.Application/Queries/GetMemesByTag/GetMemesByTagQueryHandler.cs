using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Mappings;
using SpiderMem.Persistence.Data;

namespace SpiderMem.Application.Queries.GetMemesByTag;

public class GetMemesByTagQueryHandler : IRequestHandler<GetMemesByTagQuery, Result<List<MemeDto>>>
{
    private const int PageSize = 10;
    private readonly AppDbContext _context;

    public GetMemesByTagQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<MemeDto>>> Handle(
        GetMemesByTagQuery request,
        CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;

        var memes = await _context.Memes
            .AsNoTracking()
            .Where(m => m.Tags.Any(t => t.Id == request.TagId))
            .Include(m => m.User)
            .Include(m => m.Tags)
            .Include(m => m.Comments)
                .ThenInclude(c => c.User)
            .Include(m => m.Likes)
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync(cancellationToken);

        var memeDtos = memes.ToDtos().ToList();

        return Result<List<MemeDto>>.Success(memeDtos);
    }
}
