namespace DistanceService.Infrastructure.Options;

/// <summary>
/// Настройки для кэширования. Позволяют ограничить количество
/// хранимых элементов для предотвращения переполнения памяти.
/// </summary>
public class CacheOptions
{
    /// <summary>
    /// Максимальное количество элементов, которое может содержать
    /// кэш. При достижении лимита наиболее старые элементы будут
    /// удаляться.
    /// </summary>
    public int MaxEntries { get; set; } = 1000;
}