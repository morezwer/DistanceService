namespace DistanceService.Adapters.Repositories;

public class AirportApiOptions
{
    public string BaseUrl { get; set; } = default!;
    public int CacheDurationMinutes { get; set; } = 60;
}