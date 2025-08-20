using DistanceService.Adapters.Options;
using DistanceService.Adapters.Ports;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DistanceService.Adapters.Services;

public sealed class BoundedMemoryCacheService<T> : ICacheService<T>, IDisposable
{
    private readonly MemoryCache _cache;

    public BoundedMemoryCacheService(IOptions<AdaptersOptions> options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        var maxEntries = options.Value.CacheMaxEntries;
        if (maxEntries <= 0) maxEntries = 1000;
        _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = maxEntries });
    }

    public bool TryGet(string key, out T value)
        => _cache.TryGetValue(key, out value!);

    public void Set(string key, T value, TimeSpan expiration)
    {
        var entryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration,
            Size = 1
        };
        _cache.Set(key, value, entryOptions);
    }

    public void Dispose() => _cache.Dispose();
}
