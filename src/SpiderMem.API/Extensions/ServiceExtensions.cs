using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpiderMem.API.ExceptionHandlers;
using SpiderMem.Domain.Entities;
using SpiderMem.Persistence.Data;

namespace SpiderMem.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration
                .GetConnectionString("DefaultConnection"));
        });

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddIdentityApiEndpoints<User>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}
