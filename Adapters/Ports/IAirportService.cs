using DistanceService.Domain.Entities;

namespace DistanceService.Adapters.Ports;

public interface IAirportService
{
    Task<DistanceResponse> GetDistanceAsync(string fromIata, string toIata, CancellationToken cancellationToken = default);
}
