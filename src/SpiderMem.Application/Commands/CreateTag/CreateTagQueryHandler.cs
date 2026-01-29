using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.Common;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Mappings;
using SpiderMem.Domain.Entities;
using SpiderMem.Persistence.Data;
using SpiderMem.Application.Interfaces;

namespace SpiderMem.Application.Commands.CreateTag;

public class CreateTagCommandHandler
    : IRequestHandler<CreateTagCommand, Result<TagDto>>
{
    private readonly AppDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public CreateTagCommandHandler(AppDbContext context, IUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result<TagDto>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetUserId();
        if (userId is null)
            return Result.Failure<TagDto>(Error.Unauthorized);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure<TagDto>(Error.NotFound("User"));

        var existingTag = await _context.Tags
            .AnyAsync(t => t.Name == request.Name, cancellationToken);

        if (existingTag){
            var oldtag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == request.Name);
            return Result.Success(oldtag.ToDto());
        }

        var tag = new Tag
        {
            Name = request.Name
        };

        _context.Tags.Add(tag);

        var saved = await _context.SaveChangesAsync(cancellationToken) > 0;
        if (!saved)
            return Result.Failure<TagDto>(Error.DatabaseError);

        return Result.Success(tag.ToDto());
    }
}
