namespace DistanceService.Domain.Entities;

public sealed class DistanceResponse
{
    public required string From { get; init; }
    public required string To { get; init; }
    public required double DistanceMiles { get; init; }
}