using System.Net;
using System.Text.Json;
using DistanceService.Application.Interfaces;
using DistanceService.Domain.Entities;
using DistanceService.Extensions;
using DistanceService.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace DistanceService.Infrastructure.Repositories;

/// <summary>
/// Получает данные об аэропортах из общедоступного сервиса
/// continent.ru. Результаты кэшируются, чтобы уменьшить количество
/// HTTP‑запросов и повысить производительность. При необходимости
/// данный репозиторий можно заменить реализацией, использующей
/// базу данных, зарегистрировав другой IAirportRepository в
/// контейнере зависимостей.
/// </summary>
public sealed class HttpAirportRepository : IAirportRepository
{
    private readonly HttpClient _httpClient;
    private readonly ICacheService<Airport> _cache;
    private readonly AirportApiOptions _options;

    public HttpAirportRepository(HttpClient httpClient,
                                 ICacheService<Airport> cache,
                                 IOptions<AirportApiOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        if (options == null) throw new ArgumentNullException(nameof(options));
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<Airport?> GetAirportAsync(string iata, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(iata))
        {
            throw new ArgumentException("IATA code is required", nameof(iata));
        }
        iata = iata.Trim().ToUpperInvariant();

        // Check cache first
        if (_cache.TryGet(iata, out var cached))
        {
            return cached;
        }

        // Формируем URL, используя базовый адрес из конфигурации.
        var requestUri = _options.BaseUrl + Uri.EscapeDataString(iata);

        // Perform HTTP GET
        using var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

        // If the service returns 404 simply indicate that the airport
        // was not found.
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        // Throw for any unsuccessful status codes.
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var jsonDoc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);

        var root = jsonDoc.RootElement;

        // Extract latitude and longitude from the nested location
        // object. If either field is missing we treat the airport as
        // invalid and return null.
        if (!root.TryGetProperty("location", out var locElem) ||
            !locElem.TryGetProperty("lat", out var latElem) ||
            !locElem.TryGetProperty("lon", out var lonElem) ||
            !latElem.TryGetDouble(out var lat) ||
            !lonElem.TryGetDouble(out var lon))
        {
            return null;
        }

        var airport = new Airport
        {
            Iata = iata,
            Latitude = lat,
            Longitude = lon,
            Name = root.GetPropertyOrDefault("name"),
            City = root.GetPropertyOrDefault("city"),
            Country = root.GetPropertyOrDefault("country")
        };

        // Добавляем в кэш. Продолжительность хранения берётся из
        // конфигурации (CacheDurationMinutes).
        _cache.Set(iata, airport, TimeSpan.FromMinutes(_options.CacheDurationMinutes));

        return airport;
    }
}