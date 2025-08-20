namespace DistanceService.Infrastructure.Options;

public class AirportApiOptions
{
    public string BaseUrl { get; set; } = default!;
    public int CacheDurationMinutes { get; set; } = 60;
}