using StackExchange.Redis;

namespace RinhaBackend.Infra;

public static class Cache
{
    public static async Task SetRecordAsync(
        this IConnectionMultiplexer cache,
        string key,
        string value
    )
    {
        await cache.GetDatabase(0).PublishAsync("rinha", value, CommandFlags.FireAndForget);
        await cache.GetDatabase(1).StringSetAsync(key, value);
    }

    public static async Task<string?> GetRecordAsync(
        this IConnectionMultiplexer cache, 
        string recordId
    )
    {
        return await cache.GetDatabase(1).StringGetAsync(recordId);
    }
}