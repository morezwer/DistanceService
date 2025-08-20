using System.Net;
using System.Net.Http.Json;
using DistanceService.Application.Interfaces;
using DistanceService.Domain.Entities;
using DistanceService.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace DistanceService.Infrastructure.Repositories;

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

    public async Task<Airport?> GetAirportAsync(string iata, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(iata))
        {
            throw new ArgumentException("IATA code is required", nameof(iata));
        }
        iata = iata.Trim().ToUpperInvariant();

        if (_cache.TryGet(iata, out var cached))
        {
            return cached;
        }

        var requestUri = _options.BaseUrl + Uri.EscapeDataString(iata);

        using var response = await _httpClient
            .GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var airport = await response.Content
            .ReadFromJsonAsync<Airport>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (airport?.Location is null)
        {
            return null;
        }

        airport.Iata = iata;

        _cache.Set(iata, airport, TimeSpan.FromMinutes(_options.CacheDurationMinutes));

        return airport;
    }
}