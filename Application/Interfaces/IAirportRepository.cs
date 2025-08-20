using DistanceService.Domain.Entities;

namespace DistanceService.Application.Interfaces;

public interface IAirportRepository
{
    Task<Airport?> GetAirportAsync(string iata, CancellationToken cancellationToken = default);
}