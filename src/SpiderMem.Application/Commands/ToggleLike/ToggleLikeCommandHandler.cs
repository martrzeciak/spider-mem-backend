using MediatR;
using SpiderMem.Application.Common;
using SpiderMem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Persistence.Data;

namespace SpiderMem.Application.Commands.ToggleLike;

public class ToggleLikeCommandHandler : IRequestHandler<ToggleLikeCommand, Result<int>>
{
    private readonly AppDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public ToggleLikeCommandHandler(AppDbContext context, IUserAccessor httpContextAccessor)
    {
        _context = context;
        _userAccessor = httpContextAccessor;
    }

    public async Task<Result<int>> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetUserId();
        if (userId is null)
            return Result.Failure<int>(Error.Unauthorized);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
            return Result.Failure<int>(Error.NotFound("User"));

        var meme = await _context.Memes
            .Include(m => m.Likes)
            .FirstOrDefaultAsync(m => m.Id == request.MemeId, cancellationToken);

        if (meme == null)
            return Result.Failure<int>(Error.NotFound("Meme"));

        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.MemeId == request.MemeId && l.UserId == userId, cancellationToken);

        if (existingLike != null)
        {
            _context.Likes.Remove(existingLike);
        }
        else
        {
            var like = new Like
            {
                UserId = user.Id,
                MemeId = request.MemeId
            };
            _context.Likes.Add(like);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var likesCount = await _context.Likes.CountAsync(l => l.MemeId == request.MemeId, cancellationToken);

        return Result.Success(likesCount);
    }
}
