namespace DistanceService.Adapters.Options;

public class AdaptersOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public int CacheDurationMinutes { get; set; } = 60;
    public int CacheMaxEntries { get; set; } = 1000;
}
