namespace DistanceService.Application.Options;

/// <summary>
/// Настройки, используемые при вычислении расстояний между
/// аэропортами.
/// </summary>
public class DistanceOptions
{
    /// <summary>
    /// Средний радиус Земли в милях, используемый в формуле гаверсина.
    /// При необходимости может быть изменён для учёта точных
    /// требований.
    /// </summary>
    public double EarthRadiusMiles { get; set; } = 3958.8;
}