using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Core;

public interface ICache
{
    /// <summary>
    /// Gets an object from the cache using the given key
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="token">The Cancellation Token</param>
    /// <typeparam name="T">T is a class</typeparam>
    /// <returns>An object of type T or null</returns>
    public Task<T?> GetCacheValueAsync<T>(string key, CancellationToken token = default) where T : class;
    /// <summary>
    /// Stores a value in the cache using the given key
    /// </summary>
    /// <param name="key">The key to store the value</param>
    /// <param name="value">The object to store</param>
    /// <param name="distributedCacheEntryOptions">TTL and cache options</param>
    /// <param name="token">The Cancellation Token</param>
    /// <typeparam name="T">T is a class</typeparam>
    /// <returns>Task</returns>
    public Task SetCacheValueAsync<T>(string key, T value, DistributedCacheEntryOptions? distributedCacheEntryOptions = null,CancellationToken token = default) where T : class;
    /// <summary>
    /// Removes a value from the cache using the given key
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="token">The Cancellation Token</param>
    /// <returns>Task</returns>
    public Task RemoveValueAsync(string key, CancellationToken token = default);
    /// <summary>
    /// Clear all the key/values stored by the application using it
    /// </summary>
    /// <param name="token">The Cancellation Token</param>
    /// <returns></returns>
    Task ClearCacheAsync(CancellationToken token = default);
}