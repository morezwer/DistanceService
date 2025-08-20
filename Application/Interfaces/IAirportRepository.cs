using DistanceService.Domain.Entities;

namespace DistanceService.Application.Interfaces;

/// <summary>
/// Определяет контракт для получения данных об аэропортах.
/// Реализации могут извлекать данные из удалённых HTTP‑сервисов,
/// баз данных или любых других источников. Потребители зависят
/// только от этой абстракции и не привязаны к способу получения
/// аэропортов.
/// </summary>
public interface IAirportRepository
{
    /// <summary>
    /// Пытается получить информацию об аэропорте по заданному
    /// трёхбуквенному коду IATA. Если аэропорт не найден, возвращает
    /// <c>null</c>.
    /// </summary>
    /// <param name="iata">Трёхбуквенный код аэропорта.</param>
    /// <param name="cancellationToken">Токен для отмены операции.</param>
    /// <returns>Данные об аэропорте или <c>null</c>, если не найден.</returns>
    Task<Airport?> GetAirportAsync(string iata, CancellationToken cancellationToken = default);
}