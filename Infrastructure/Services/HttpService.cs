using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using DistanceService.Application.Interfaces;

namespace DistanceService.Infrastructure.Services;

/// <summary>
/// Реализация универсального HTTP-сервиса на основе <see cref="HttpClient"/>.
/// </summary>
public sealed class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;

    public HttpService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <inheritdoc />
    public Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        => _httpClient.GetFromJsonAsync<T>(url, cancellationToken);

    /// <inheritdoc />
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(url, data, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(url, data, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync(url, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}
