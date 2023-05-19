using Microsoft.Extensions.Caching.Distributed;

namespace Distributed.Cache.Core;

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
    /// Gets an string from the cache using the given key
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="token">The Cancellation Token</param>
    /// <returns></returns>
    Task<string?> GetCacheValueAsync(string key, CancellationToken token = default);
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
    /// Stores a value in the cache using the given key and the default TTL
    /// </summary>
    /// <param name="key">The key to store the value</param>
    /// <param name="value">The object to store</param>
    /// <param name="token">The Cancellation Token</param>
    /// <typeparam name="T">T is a class</typeparam>
    /// <returns>Task</returns>
    public Task SetCacheValueAsync<T>(string key, T value, CancellationToken token = default) where T : class;
    /// <summary>
    /// Stores a value in the cache using the given key
    /// </summary>
    /// <param name="key">The key to store the value</param>
    /// <param name="value">The string to store</param>
    /// <param name="distributedCacheEntryOptions">TTL and cache options</param>
    /// <param name="token">The Cancellation Token</param>
    /// <returns>Task</returns>
    Task SetCacheValueAsync(string key, string value, DistributedCacheEntryOptions? distributedCacheEntryOptions = null, CancellationToken token = default);
    /// <summary>
    /// Stores a value in the cache using the given key and the default TTL
    /// </summary>
    /// <param name="key">The key to store the value</param>
    /// <param name="value">The string to store</param>
    /// <param name="token">The Cancellation Token</param>
    /// <returns>Task</returns>
    Task SetCacheValueAsync(string key, string value, CancellationToken token = default);
    /// <summary>
    /// Removes a value from the cache using the given key
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="token">The Cancellation Token</param>
    /// <returns>Task</returns>
    public Task RemoveValueAsync(string key, CancellationToken token = default);
    /// <summary>
    /// Removes all values from the cache which starts with the given prefix
    /// </summary>
    /// <param name="prefix">The key to search for</param>
    /// <param name="token">The Cancellation Token</param>
    /// <returns></returns>
    Task ClearCacheWithPrefixAsync(string prefix, CancellationToken token = default);
    /// <summary>
    /// Clear all the key/values stored by the application having the passed prefix
    /// </summary>
    /// <param name="token">The Cancellation Token</param>
    /// <returns></returns>
    Task ClearCacheAsync(CancellationToken token = default);
}