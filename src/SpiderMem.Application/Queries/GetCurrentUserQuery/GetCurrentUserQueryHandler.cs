using MediatR;
using SpiderMem.Application.DTOs;
using SpiderMem.Persistence.Data;
using SpiderMem.Application.Common;

namespace SpiderMem.Application.Queries.GetCurrentUser;

using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.Mappings;

public class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
{
    private readonly AppDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public GetCurrentUserQueryHandler(AppDbContext context, IUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result<UserDto>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetUserId();
        if (userId is null)
            return Result.Failure<UserDto>(Error.Unauthorized);

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return Result.Failure<UserDto>(Error.NotFound("User"));

        return Result.Success(user.ToDto());
    }
}
