namespace DistanceService.Application.Interfaces;

/// <summary>
/// Определяет простую абстракцию кэша, позволяющую получать и
/// сохранять значения по ключу с возможностью указания срока
/// действия. Эта абстракция отделяет потребителей от конкретной
/// реализации кэша и позволяет при необходимости заменить её.
/// </summary>
/// <typeparam name="T">Тип хранимых значений.</typeparam>
public interface ICacheService<T>
{
    /// <summary>
    /// Пытается получить значение из кэша.
    /// </summary>
    /// <param name="key">Уникальный ключ, ассоциированный с
    /// сохранённым значением.</param>
    /// <param name="value">Сохранённое значение, если оно присутствует и
    /// ещё не истекло.</param>
    /// <returns><c>true</c>, если актуальное значение найдено в кэше;
    /// иначе <c>false</c>.</returns>
    bool TryGet(string key, out T value);

    /// <summary>
    /// Добавляет или обновляет значение в кэше.
    /// </summary>
    /// <param name="key">Ключ, по которому будет храниться значение.</param>
    /// <param name="value">Значение, которое нужно сохранить.</param>
    /// <param name="expiration">Длительность жизни записи, после
    /// которой она считается устаревшей.</param>
    void Set(string key, T value, TimeSpan expiration);
}