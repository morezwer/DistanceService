using System.Text.Json.Serialization;

namespace DistanceService.Domain.Entities;

public sealed class Airport
{
    [JsonIgnore]
    public string? Iata { get; set; }

    [JsonIgnore]
    public double Latitude { get; private set; }

    [JsonIgnore]
    public double Longitude { get; private set; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("city")]
    public string? City { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("location")]
    public Coordinates? Location
    {
        get => new(Latitude, Longitude);
        init
        {
            if (value != null)
            {
                Latitude = value.Latitude;
                Longitude = value.Longitude;
            }
        }
    }

    public sealed record Coordinates(
        [property: JsonPropertyName("lat")] double Latitude,
        [property: JsonPropertyName("lon")] double Longitude
    );
}