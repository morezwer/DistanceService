using System.Text.Json.Serialization;

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
    /// Значение не приходит из внешнего сервиса и заполняется вручную.
    /// </summary>
    [JsonIgnore]
    public string? Iata { get; set; }

    /// <summary>
    /// Географическая широта аэропорта в градусах.
    /// </summary>
    [JsonIgnore]
    public double Latitude { get; private set; }

    /// <summary>
    /// Географическая долгота аэропорта в градусах.
    /// </summary>
    [JsonIgnore]
    public double Longitude { get; private set; }

    /// <summary>
    /// Человекочитаемое название аэропорта.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Город, в котором расположен аэропорт.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// Страна, в которой находится аэропорт.
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; init; }

    /// <summary>
    /// Объект, используемый для (де)сериализации координат.
    /// </summary>
    [JsonPropertyName("location")]
    public Coordinates? Location
    {
        get => new(Latitude, Longitude);
        init
        {
            if (value != null)
            {
                Latitude = value.Latitude;
                Longitude = value.Longitude;
            }
        }
    }

    /// <summary>
    /// Вспомогательная структура для представления координат в JSON.
    /// </summary>
    public sealed record Coordinates(
        [property: JsonPropertyName("lat")] double Latitude,
        [property: JsonPropertyName("lon")] double Longitude
    );
}