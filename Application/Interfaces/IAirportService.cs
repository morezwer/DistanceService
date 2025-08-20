using DistanceService.Domain.Entities;

namespace DistanceService.Application.Interfaces;

/// <summary>
/// Предоставляет высокоуровневый сервис для вычисления расстояния
/// между двумя аэропортами. Потребители взаимодействуют с этой
/// абстракцией вместо прямого обращения к репозиторию, что
/// централизует вычисления расстояний и валидацию входных данных.
/// </summary>
public interface IAirportService
{
    /// <summary>
    /// Вычисляет расстояние (по большой окружности) в милях между
    /// двумя аэропортами, идентифицируемыми их IATA кодами.
    /// </summary>
    /// <param name="fromIata">IATA‑код аэропорта отправления.</param>
    /// <param name="toIata">IATA‑код аэропорта назначения.</param>
    /// <param name="cancellationToken">Токен для отмены операции.</param>
    /// <returns>Экземпляр <see cref="DistanceResponse"/>, содержащий
    /// рассчитанное расстояние.</returns>
    Task<DistanceResponse> GetDistanceAsync(string fromIata, string toIata, CancellationToken cancellationToken = default);
}