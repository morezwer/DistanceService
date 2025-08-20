namespace DistanceService.Domain.Entities;

/// <summary>
/// Представляет базовую информацию об аэропорте. Здесь определены
/// только поля, необходимые для расчёта расстояния. Дополнительные
/// свойства из внешнего сервиса можно добавлять по мере
/// необходимости, не затрагивая потребителей, зависящих от этой
/// абстракции.
/// </summary>
public sealed class Airport
{
    /// <summary>
    /// Трёхбуквенный код IATA, уникально идентифицирующий аэропорт.
    /// </summary>
    public required string Iata { get; init; }

    /// <summary>
    /// Географическая широта аэропорта в градусах.
    /// </summary>
    public required double Latitude { get; init; }

    /// <summary>
    /// Географическая долгота аэропорта в градусах.
    /// </summary>
    public required double Longitude { get; init; }

    /// <summary>
    /// Человекочитаемое название аэропорта.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Город, в котором расположен аэропорт.
    /// </summary>
    public string? City { get; init; }

    /// <summary>
    /// Страна, в которой находится аэропорт.
    /// </summary>
    public string? Country { get; init; }
}