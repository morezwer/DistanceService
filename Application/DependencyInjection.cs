using DistanceService.Application.Interfaces;
using DistanceService.Application.Options;
using DistanceService.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DistanceOptions>(configuration.GetSection("Distance"));
        services.AddScoped<IAirportService, AirportService>();
        return services;
    }
}
