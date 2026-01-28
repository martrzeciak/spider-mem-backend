using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Mappings;
using SpiderMem.Domain.Entities;
using SpiderMem.Persistence.Data;
using SpiderMem.Application.Interfaces;

namespace SpiderMem.Application.Commands.CreateMeme;

public class CreateMemeCommandHandler
    : IRequestHandler<CreateMemeCommand, Result<MemeDto>>
{
    private readonly AppDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public CreateMemeCommandHandler(
        AppDbContext context,
        IUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result<MemeDto>> Handle(
        CreateMemeCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetUserId();
        if (userId is null)
            return Result.Failure<MemeDto>(Error.Unauthorized);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure<MemeDto>(Error.NotFound("User"));

        var tags = await _context.Tags
            .Where(t => request.TagIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        var meme = new Meme
        {
            Title = request.Title,
            ImageUrl = request.ImageUrl,
            UserId = user.Id,
            User = user,
            Tags = tags
        };

        _context.Memes.Add(meme);

        var saved = await _context.SaveChangesAsync(cancellationToken) > 0;
        if (!saved)
            return Result.Failure<MemeDto>(Error.DatabaseError);

        return Result.Success(meme.ToDto());
    }
}
