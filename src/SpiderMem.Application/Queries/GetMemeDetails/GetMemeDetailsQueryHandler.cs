using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.DTOs;
using SpiderMem.Persistence.Data;
using SpiderMem.Application.Common;

namespace SpiderMem.Application.Queries.GetMemeDetails;

public class GetMemeDetailsQueryHandler : IRequestHandler<GetMemeDetailsQuery, Result<MemeDto>>
{
    private readonly AppDbContext _context;

    public GetMemeDetailsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<MemeDto>> Handle(
        GetMemeDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var meme = await _context.Memes
            .AsNoTracking()
            .Where(m => m.Id == request.MemeId)
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
                        c.MemeId,
                        c.User!.UserName!
                    ))
                    .ToList(),
                m.Likes.Count
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (meme is null) return Result.Failure<MemeDto>(Error.NotFound("Meme"));

        return Result<MemeDto>.Success(meme);
    }
}
