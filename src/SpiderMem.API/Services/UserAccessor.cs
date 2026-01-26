using System.Security.Claims;
using SpiderMem.Application.Common;

namespace SpiderMem.API.Services;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetUserId()
    {
        var id = _httpContextAccessor.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

        return id is null ? null : Guid.Parse(id);
    }
}
