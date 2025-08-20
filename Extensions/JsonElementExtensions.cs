using System.Text.Json;

namespace DistanceService.Extensions;

/// <summary>
/// Extension methods for <see cref="JsonElement"/> simplifying retrieval of optional string properties.
/// </summary>
internal static class JsonElementExtensions
{
    /// <summary>
    /// Attempts to get the specified property as a string. Returns null if property is missing or not a string.
    /// </summary>
    public static string? GetPropertyOrDefault(this JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String)
        {
            return prop.GetString();
        }
        return null;
    }
}
