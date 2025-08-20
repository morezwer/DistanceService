using DistanceService.Domain.Entities;

namespace DistanceService.Domain;

public interface IAirportRepository
{
    Task<Airport?> GetAirportAsync(string iata, CancellationToken cancellationToken = default);
}