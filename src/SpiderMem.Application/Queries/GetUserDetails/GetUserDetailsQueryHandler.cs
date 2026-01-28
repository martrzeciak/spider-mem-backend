using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.DTOs;
using SpiderMem.Persistence.Data;
using SpiderMem.Application.Common;

namespace SpiderMem.Application.Queries.GetUserDetails;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, Result<UserDetailsDto>>
{
    private readonly AppDbContext _context;

    public GetUserDetailsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDetailsDto>> Handle(
        GetUserDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == request.Id)
            .Select(u => new UserDetailsDto(
                u.Id,
                u.UserName,
                u.Email,
                u.Memes.Count
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null) return Result.Failure<UserDetailsDto>(Error.NotFound("User"));

        return Result<UserDetailsDto>.Success(user);
    }
}
