using System.Collections.Concurrent;
using DistanceService.Application.Interfaces;

namespace DistanceService.Infrastructure.Services;

/// <summary>
/// Простая потокобезопасная реализация кэша в памяти, которая
/// сохраняет значения по ключу с фиксированным временем истечения.
/// Реализация лёгкая и не зависит от внешних библиотек, что делает
/// её удобной для демонстрационных целей. В производстве следует
/// рассмотреть более надёжное решение для кэширования.
/// </summary>
/// <typeparam name="T">Тип значений, помещаемых в кэш.</typeparam>
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

    /// <inheritdoc />
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
            // Remove expired entry.
            _items.TryRemove(key, out _);
        }
        return false;
    }

    /// <inheritdoc />
    public void Set(string key, T value, TimeSpan expiration)
    {
        var expiresAt = DateTimeOffset.UtcNow.Add(expiration);
        var entry = new CacheEntry(value, expiresAt);
        _items[key] = entry;
    }
}