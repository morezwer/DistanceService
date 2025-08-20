using DistanceService.Application.Interfaces;
using DistanceService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DistanceService.Controllers;

/// <summary>
/// Предоставляет HTTP API для вычисления расстояния между двумя
/// аэропортами. Контроллер делегирует выполнение расчётов
/// экземпляру <see cref="IAirportService"/>.
/// </summary>
// Контроллер API, защищён авторизацией. Все запросы к нему
// требуют передачи валидного JWT токена в заголовке Authorization.
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class DistanceController : ControllerBase
{
    private readonly IAirportService _airportService;

    public DistanceController(IAirportService airportService)
    {
        _airportService = airportService ?? throw new ArgumentNullException(nameof(airportService));
    }

    /// <summary>
    /// Возвращает расстояние между двумя аэропортами (по большой
    /// окружности) в милях.
    /// </summary>
    /// <param name="from">IATA‑код аэропорта отправления.</param>
    /// <param name="to">IATA‑код аэропорта назначения.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Экземпляр <see cref="DistanceResponse"/>, содержащий
    /// рассчитанное расстояние.</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string from, [FromQuery] string to, CancellationToken cancellationToken)
    {
        var result = await _airportService.GetDistanceAsync(from, to, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }
}