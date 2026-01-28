using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Persistence.Data;

namespace SpiderMem.Application.Queries.GetMemes;

public class GetMemesQueryHandler : IRequestHandler<GetMemesQuery, Result<PagedList<MemeDto>>>{
    private const int PageSize = 10;

    private readonly AppDbContext _context;

    public GetMemesQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedList<MemeDto>>> Handle(GetMemesQuery request, CancellationToken cancellationToken){

        var memes = _context.Memes
            .AsNoTracking()
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
