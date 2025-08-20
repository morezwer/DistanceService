using DistanceService.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DistanceService.Controllers;

/// <summary>
/// Простейший контроллер, выдающий JWT токен для указанного пользователя.
/// В реальном приложении следует проверять учётные данные пользователя.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly JwtOptions _options;
    private readonly string _secret;

    public AuthController(IOptions<JwtOptions> options, IConfiguration configuration)
    {
        _options = options.Value;
        _secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret not configured.");
    }

    /// <summary>
    /// Возвращает JWT токен.
    /// </summary>
    /// <param name="request">Имя пользователя.</param>
    /// <returns>JWT токен.</returns>
    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult CreateToken([FromBody] LoginRequest request)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, request.Username) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    /// <summary>
    /// Запрос на выдачу токена.
    /// </summary>
    public record LoginRequest(string Username);
}
