namespace DistanceService.Infrastructure.Options;

/// <summary>
/// Параметры, необходимые для конфигурации клиента аэропортов. Эти
/// параметры загружаются из файла конфигурации (appsettings.json)
/// посредством механизма Options.
/// </summary>
public class AirportApiOptions
{
    /// <summary>
    /// Базовый адрес внешнего сервиса, предоставляющего данные об
    /// аэропортах. Должен заканчиваться косой чертой. Например:
    /// "https://places-dev.continent.ru/airports/".
    /// </summary>
    public string BaseUrl { get; set; } = "https://places-dev.continent.ru/airports/";

    /// <summary>
    /// Длительность хранения данных аэропортов в кэше (в минутах). По
    /// истечении этого времени информация будет повторно загружена
    /// из внешнего сервиса.
    /// </summary>
    public int CacheDurationMinutes { get; set; } = 60;
}