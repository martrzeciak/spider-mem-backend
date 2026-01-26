using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Domain.Entities;
using SpiderMem.Persistence.Data;
using SpiderMem.Application.Mappings;

namespace SpiderMem.Application.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
{
    private readonly AppDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public CreateCommentCommandHandler(AppDbContext context, IUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetUserId();
        if (userId is null)
            return Result.Failure<CommentDto>(Error.Unauthorized);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
            return Result.Failure<CommentDto>(Error.NotFound("User"));

        var meme = await _context.Memes
            .Include(m => m.Comments)
            .FirstOrDefaultAsync(m => m.Id == request.CommentDto.MemeId, cancellationToken);

        if (meme == null)
            return Result.Failure<CommentDto>(Error.NotFound("Meme"));

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Content = request.CommentDto.Content,
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id,
            User = user
        };

        meme.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(comment.ToDto());
    }
}