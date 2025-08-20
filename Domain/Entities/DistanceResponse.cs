namespace DistanceService.Domain.Entities;

/// <summary>
/// Представляет результат вычисления расстояния между двумя
/// аэропортами. Этот DTO возвращается API и намеренно содержит
/// только необходимые поля.
/// </summary>
public sealed class DistanceResponse
{
    /// <summary>
    /// Код IATA аэропорта отправления.
    /// </summary>
    public required string From { get; init; }

    /// <summary>
    /// Код IATA аэропорта назначения.
    /// </summary>
    public required string To { get; init; }

    /// <summary>
    /// Расстояние (по большой окружности) между аэропортами в милях.
    /// </summary>
    public required double DistanceMiles { get; init; }
}