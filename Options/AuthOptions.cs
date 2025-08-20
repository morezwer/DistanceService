namespace DistanceService.Options;

/// <summary>
/// Настройки авторизации. Содержит секретный код, который
/// должен быть передан в заголовке Authorization в формате
/// Bearer <token> для доступа к защищённым ручкам.
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// Секретный токен, который клиент должен отправлять в
    /// заголовке Authorization для прохождения аутентификации.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}