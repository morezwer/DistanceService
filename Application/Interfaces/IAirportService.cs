using DistanceService.Domain.Entities;

namespace DistanceService.Application.Interfaces;

public interface IAirportService
{
    Task<DistanceResponse> GetDistanceAsync(string fromIata, string toIata, CancellationToken cancellationToken = default);
}