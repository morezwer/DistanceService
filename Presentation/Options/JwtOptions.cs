namespace DistanceService.Presentation.Options;

/// <summary>
/// Параметры JWT аутентификации, не содержащие секретных данных.
/// Секретный ключ передаётся через переменную окружения <c>Jwt__Secret</c>.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Издатель токена.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Потребитель токена.
    /// </summary>
    public string Audience { get; set; } = string.Empty;
}
