using SpiderMem.API.ExceptionHandlers;

namespace SpiderMem.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
}
