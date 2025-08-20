using System.Collections.Concurrent;
using DistanceService.Application.Interfaces;

namespace DistanceService.Infrastructure.Services;

public sealed class InMemoryCacheService<T> : ICacheService<T>
{
    private sealed class CacheEntry
    {
        public CacheEntry(T value, DateTimeOffset expiration)
        {
            Value = value;
            Expiration = expiration;
        }

        public T Value { get; }
        public DateTimeOffset Expiration { get; }
    }

    private readonly ConcurrentDictionary<string, CacheEntry> _items = new();

    public bool TryGet(string key, out T value)
    {
        value = default!;
        if (_items.TryGetValue(key, out var entry))
        {
            if (entry.Expiration > DateTimeOffset.UtcNow)
            {
                value = entry.Value;
                return true;
            }

            _items.TryRemove(key, out _);
        }
        return false;
    }

    public void Set(string key, T value, TimeSpan expiration)
    {
        var expiresAt = DateTimeOffset.UtcNow.Add(expiration);
        var entry = new CacheEntry(value, expiresAt);
        _items[key] = entry;
    }
}