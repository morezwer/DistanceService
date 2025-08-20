using System.Threading;
using System.Threading.Tasks;

namespace DistanceService.Adapters.Ports;

/// <summary>
/// Универсальный HTTP-сервис для выполнения запросов и (де)сериализации ответов.
/// </summary>
public interface IHttpService
{
    Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancellationToken = default);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancellationToken = default);
    Task DeleteAsync(string url, CancellationToken cancellationToken = default);
}
