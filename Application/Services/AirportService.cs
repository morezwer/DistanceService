using DistanceService.Application.Interfaces;
using DistanceService.Application.Options;
using DistanceService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace DistanceService.Application.Services;

public sealed class AirportService : IAirportService
{
    private readonly IAirportRepository _repository;
    private readonly double _earthRadiusMiles;

    public AirportService(IAirportRepository repository, IOptions<DistanceOptions> distanceOptions)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        if (distanceOptions == null) throw new ArgumentNullException(nameof(distanceOptions));
        _earthRadiusMiles = distanceOptions.Value.EarthRadiusMiles;
    }

    public async Task<DistanceResponse> GetDistanceAsync(string fromIata, string toIata, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fromIata))
            throw new ArgumentException("Origin IATA code is required", nameof(fromIata));
        if (string.IsNullOrWhiteSpace(toIata))
            throw new ArgumentException("Destination IATA code is required", nameof(toIata));

        fromIata = fromIata.Trim().ToUpperInvariant();
        toIata = toIata.Trim().ToUpperInvariant();

        var fromAirport = await _repository.GetAirportAsync(fromIata, cancellationToken).ConfigureAwait(false);
        if (fromAirport is null)
        {
            throw new KeyNotFoundException($"Airport '{fromIata}' was not found.");
        }

        var toAirport = await _repository.GetAirportAsync(toIata, cancellationToken).ConfigureAwait(false);
        if (toAirport is null)
        {
            throw new KeyNotFoundException($"Airport '{toIata}' was not found.");
        }

        var distance = CalculateDistanceInMiles(fromAirport.Latitude, fromAirport.Longitude, toAirport.Latitude, toAirport.Longitude);

        return new DistanceResponse
        {
            From = fromIata,
            To = toIata,
            DistanceMiles = distance
        };
    }

    private double CalculateDistanceInMiles(double lat1, double lon1, double lat2, double lon2)
    {
        double ToRadians(double degrees) => Math.PI * degrees / 180.0;

        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Pow(Math.Sin(dLat / 2.0), 2.0) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Pow(Math.Sin(dLon / 2.0), 2.0);
        var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

        return _earthRadiusMiles * c;
    }
}