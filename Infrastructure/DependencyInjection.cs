using DistanceService.Application.Interfaces;
using DistanceService.Infrastructure.Options;
using DistanceService.Infrastructure.Repositories;
using DistanceService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AirportApiOptions>(configuration.GetSection("AirportApi"));
        services.Configure<CacheOptions>(configuration.GetSection("Cache"));

        services.AddHttpClient<IAirportRepository, HttpAirportRepository>();
        services.AddHttpClient<IHttpService, HttpService>();
        services.AddSingleton(typeof(ICacheService<>), typeof(BoundedMemoryCacheService<>));

        return services;
    }
}
