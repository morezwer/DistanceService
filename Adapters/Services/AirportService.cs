using DistanceService.Adapters.Ports;
using DistanceService.Domain;
using DistanceService.Domain.Entities;

namespace DistanceService.Adapters.Services;

public sealed class AirportService : IAirportService
{
    private readonly IAirportRepository _repository;

    public AirportService(IAirportRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
            throw new KeyNotFoundException($"Airport '{fromIata}' was not found.");

        var toAirport = await _repository.GetAirportAsync(toIata, cancellationToken).ConfigureAwait(false);
        if (toAirport is null)
            throw new KeyNotFoundException($"Airport '{toIata}' was not found.");

        var distance = DistanceCalculator.CalculateMiles(fromAirport.Latitude, fromAirport.Longitude,
                                                         toAirport.Latitude, toAirport.Longitude);

        return new DistanceResponse
        {
            From = fromIata,
            To = toIata,
            DistanceMiles = distance
        };
    }
}
