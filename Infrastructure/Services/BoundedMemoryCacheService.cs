using System.Collections.Concurrent;
using DistanceService.Application.Interfaces;
using DistanceService.Options;
using Microsoft.Extensions.Options;

namespace DistanceService.Infrastructure.Services;

/// <summary>
/// Потокобезопасный кэш в памяти с ограничением на количество
/// элементов. При достижении указанной ёмкости кэш удаляет
/// устаревшие или самые старые элементы, чтобы освободить место.
/// </summary>
/// <typeparam name="T">Тип хранимых значений.</typeparam>
public sealed class BoundedMemoryCacheService<T> : ICacheService<T>
{
    private sealed class CacheEntry
    {
        public CacheEntry(T value, DateTimeOffset expiration, DateTimeOffset created)
        {
            Value = value;
            Expiration = expiration;
            Created = created;
        }

        public T Value { get; }
        public DateTimeOffset Expiration { get; }
        public DateTimeOffset Created { get; }
    }

    private readonly ConcurrentDictionary<string, CacheEntry> _items = new();
    private readonly int _maxEntries;

    public BoundedMemoryCacheService(IOptions<CacheOptions> options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        _maxEntries = options.Value.MaxEntries;
        if (_maxEntries <= 0) _maxEntries = 1000;
    }

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
            // удаляем просроченную запись
            _items.TryRemove(key, out _);
        }
        return false;
    }

    /// <inheritdoc />
    public void Set(string key, T value, TimeSpan expiration)
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.Add(expiration);
        var entry = new CacheEntry(value, expiresAt, now);

        _items[key] = entry;

        EvictIfNecessary();
    }

    /// <summary>
    /// Удаляет элементы из кэша, если он превысил максимальную ёмкость.
    /// В первую очередь удаляются просроченные записи, затем самые старые.
    /// </summary>
    private void EvictIfNecessary()
    {
        // если размер в пределах лимита, ничего делать не надо
        while (_items.Count > _maxEntries)
        {
            // пытаемся удалить просроченные записи
            foreach (var kvp in _items)
            {
                if (kvp.Value.Expiration <= DateTimeOffset.UtcNow)
                {
                    _items.TryRemove(kvp.Key, out _);
                    // удалили одну запись, проверяем лимит
                    if (_items.Count <= _maxEntries) return;
                }
            }

            // если всё ещё много элементов, удаляем самую старую
            var oldest = _items.OrderBy(k => k.Value.Created).FirstOrDefault();
            if (!string.IsNullOrEmpty(oldest.Key))
            {
                _items.TryRemove(oldest.Key, out _);
            }
            else
            {
                // если не найдено (не должно случиться), выходим
                break;
            }
        }
    }
}