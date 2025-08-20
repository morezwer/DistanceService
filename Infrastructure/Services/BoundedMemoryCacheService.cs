using DistanceService.Application.Interfaces;
using DistanceService.Infrastructure.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DistanceService.Infrastructure.Services;

/// <summary>
/// Потокобезопасный кэш в памяти с ограничением на количество элементов.
/// Использует <see cref="MemoryCache"/> для автоматического удаления
/// просроченных записей и контроля размера кэша.
/// </summary>
/// <typeparam name="T">Тип хранимых значений.</typeparam>
public sealed class BoundedMemoryCacheService<T> : ICacheService<T>, IDisposable
{
    private readonly MemoryCache _cache;

    public BoundedMemoryCacheService(IOptions<CacheOptions> options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        var maxEntries = options.Value.MaxEntries;
        if (maxEntries <= 0) maxEntries = 1000;
        _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = maxEntries });
    }

    /// <inheritdoc />
    public bool TryGet(string key, out T value)
    {
        return _cache.TryGetValue(key, out value!);
    }

    /// <inheritdoc />
    public void Set(string key, T value, TimeSpan expiration)
    {
        var entryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration,
            Size = 1
        };
        _cache.Set(key, value, entryOptions);
    }

    public void Dispose()
    {
        _cache.Dispose();
    }
}
