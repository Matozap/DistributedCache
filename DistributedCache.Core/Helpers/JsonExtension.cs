using System.Text.Json;

namespace DistributedCache.Core.Helpers;

public static class JsonExtension
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    internal static T? Deserialize<T>(this string json) => JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    internal static string Serialize<T>(this T obj) => JsonSerializer.Serialize(obj, JsonSerializerOptions);
}