namespace DistanceService.Application.Interfaces;

public interface ICacheService<T>
{
    bool TryGet(string key, out T value);
    void Set(string key, T value, TimeSpan expiration);
}