using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Mappings;
using SpiderMem.Persistence.Data;
using SpiderMem.Application.Common;

namespace SpiderMem.Application.Queries.GetMemesByTag;

public class GetMemesByTagQueryHandler : IRequestHandler<GetMemesByTagQuery, Result<PagedList<MemeDto>>>
{
    private readonly AppDbContext _context;

    public GetMemesByTagQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedList<MemeDto>>> Handle(
        GetMemesByTagQuery request,
        CancellationToken cancellationToken)
    {
        var memes = _context.Memes
            .AsNoTracking()
            .Where(m => m.Tags.Any(t => t.Id == request.TagId))
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MemeDto(
                m.Id,
                m.Title,
                m.ImageUrl,
                m.CreatedAt,
                m.User.Id,
                m.User.UserName,
                m.Tags
                    .Select(mt => new TagDto(
                        mt.Id,
                        mt.Name))
                    .ToList(),
                m.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CommentDto(
                        c.Id,
                        c.Content,
                        c.CreatedAt,
                        c.User.Id,
                        c.MemeId,
                        c.User.UserName))
                    .ToList(),
                m.Likes.Count
            ))
            .AsQueryable();

        return Result.Success(await PagedList<MemeDto>
            .CreateAsync(memes, request.MemeParams.PageNumber, 
                request.MemeParams.PageSize));
    }
}
