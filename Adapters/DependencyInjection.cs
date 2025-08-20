using DistanceService.Adapters.Options;
using DistanceService.Adapters.Ports;
using DistanceService.Adapters.Repositories;
using DistanceService.Adapters.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceService.Adapters;

public static class DependencyInjection
{
    public static IServiceCollection AddAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AdaptersOptions>(configuration.GetSection("Adapters"));
        services.AddHttpClient<IAirportRepository, HttpAirportRepository>();
        services.AddHttpClient<IHttpService, HttpService>();
        services.AddSingleton(typeof(ICacheService<>), typeof(BoundedMemoryCacheService<>));
        services.AddScoped<IAirportService, AirportService>();
        return services;
    }
}
