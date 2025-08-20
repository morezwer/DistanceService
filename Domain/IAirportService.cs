using DistanceService.Domain.Entities;

namespace DistanceService.Domain;

public interface IAirportService
{
    Task<DistanceResponse> GetDistanceAsync(string fromIata, string toIata, CancellationToken cancellationToken = default);
}