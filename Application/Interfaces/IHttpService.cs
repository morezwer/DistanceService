using System.Threading;
using System.Threading.Tasks;

namespace DistanceService.Application.Interfaces;

/// <summary>
/// Универсальный HTTP-сервис для выполнения запросов и (де)сериализации ответов.
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Выполняет HTTP GET-запрос по указанному адресу и десериализует ответ.
    /// </summary>
    /// <typeparam name="T">Тип ожидаемого ответа.</typeparam>
    /// <param name="url">Адрес запроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполняет HTTP POST-запрос с указанными данными и десериализует ответ.
    /// </summary>
    /// <typeparam name="TRequest">Тип отправляемых данных.</typeparam>
    /// <typeparam name="TResponse">Тип ожидаемого ответа.</typeparam>
    /// <param name="url">Адрес запроса.</param>
    /// <param name="data">Отправляемые данные.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполняет HTTP PUT-запрос с указанными данными и десериализует ответ.
    /// </summary>
    /// <typeparam name="TRequest">Тип отправляемых данных.</typeparam>
    /// <typeparam name="TResponse">Тип ожидаемого ответа.</typeparam>
    /// <param name="url">Адрес запроса.</param>
    /// <param name="data">Отправляемые данные.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполняет HTTP DELETE-запрос по указанному адресу.
    /// </summary>
    /// <param name="url">Адрес запроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(string url, CancellationToken cancellationToken = default);
}
