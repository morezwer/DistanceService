using DistanceService.Domain.Entities;

namespace DistanceService.Adapters.Ports;

public interface IAirportRepository
{
    Task<Airport?> GetAirportAsync(string iata, CancellationToken cancellationToken = default);
}
