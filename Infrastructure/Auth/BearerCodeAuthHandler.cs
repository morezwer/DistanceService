using System.Security.Claims;
using System.Text.Encodings.Web;
using DistanceService.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DistanceService.Infrastructure.Auth;

/// <summary>
/// Специализированный обработчик аутентификации, который проверяет
/// наличие заголовка Authorization вида <c>Bearer &lt;токен&gt;</c> и
/// сравнивает токен с заранее настроенным значением из конфигурации.
/// Если токен совпадает, запрос считается аутентифицированным.
/// </summary>
public sealed class BearerCodeAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AuthOptions _authOptions;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BearerCodeAuthHandler"/>.
    /// </summary>
    /// <param name="options">Опции схемы аутентификации.</param>
    /// <param name="logger">Логер.</param>
    /// <param name="encoder">Энкодер URL.</param>
    /// <param name="clock">Системные часы.</param>
    /// <param name="authOptions">Параметры авторизации, включая ожидаемый токен.</param>
    public BearerCodeAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IOptions<AuthOptions> authOptions)
        : base(options, logger, encoder, clock)
    {
        _authOptions = authOptions.Value;
    }

    /// <summary>
    /// Выполняет проверку аутентификации. Метод пытается извлечь
    /// токен из заголовка Authorization и сравнить его с ожидаемым.
    /// </summary>
    /// <returns>Результат аутентификации.</returns>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Отсутствие заголовка Authorization интерпретируется как отказ в доступе.
        if (!Request.Headers.TryGetValue("Authorization", out var headerValues))
        {
            return Task.FromResult(AuthenticateResult.Fail("Отсутствует заголовок Authorization."));
        }

        var header = headerValues.ToString();
        const string prefix = "Bearer ";
        if (!header.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Неверный формат заголовка Authorization."));
        }

        // Извлекаем токен из заголовка
        var token = header.Substring(prefix.Length).Trim();

        // Сравниваем с токеном из конфигурации
        if (string.IsNullOrEmpty(_authOptions.Token) || !string.Equals(token, _authOptions.Token, StringComparison.Ordinal))
        {
            return Task.FromResult(AuthenticateResult.Fail("Неверный токен."));
        }

        // Создаём ClaimsIdentity и отмечаем пользователя как успешно аутентифицированного
        var claims = new[] { new Claim(ClaimTypes.Name, "ApiUser") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}