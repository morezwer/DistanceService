using System.Text.Json.Serialization;

namespace DistanceService.Infrastructure.Repositories;

/// <summary>
/// DTO representing response from external airport API.
/// </summary>
internal sealed class AirportApiDto
{
    /// <summary>
    /// Airport name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// City where airport is located.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// Country of the airport.
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; init; }

    /// <summary>
    /// Geographical location information.
    /// </summary>
    [JsonPropertyName("location")]
    public LocationDto? Location { get; init; }

    internal sealed class LocationDto
    {
        [JsonPropertyName("lat")]
        public double? Latitude { get; init; }

        [JsonPropertyName("lon")]
        public double? Longitude { get; init; }
    }
}
