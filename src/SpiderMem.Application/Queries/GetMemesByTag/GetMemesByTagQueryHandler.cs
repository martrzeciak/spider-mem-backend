using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.DTOs;
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
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(m => new MemeDto(
                m.Id,
                m.Title,
                m.ImageUrl,
                m.CreatedAt,
                m.UserId,
                m.User!.UserName!,
                m.Tags
                    .Select(t => new TagDto(
                        t.Id,
                        t.Name))
                    .ToList(),
                m.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CommentDto(
                        c.Id,
                        c.Content,
                        c.CreatedAt,
                        c.UserId,
                        c.User!.UserName!
                    ))
                    .ToList(),
                m.Likes.Count
            ))
            .ToListAsync(cancellationToken);

        return Result<List<MemeDto>>.Success(memes);
    }
}
