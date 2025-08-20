using DistanceService.Adapters.Ports;
using DistanceService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistanceService.Presentation.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string from, [FromQuery] string to, CancellationToken cancellationToken)
    {
        var result = await _airportService.GetDistanceAsync(from, to, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }
}
