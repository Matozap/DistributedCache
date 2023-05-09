using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Core.Configuration;

public class CacheOptions
{
    public record HealthCheckOptions(bool Enabled, int MaxErrorsAllowed, int ResetIntervalMinutes);

    /// <summary>
    /// Contains the cache type (Read-Only) - Use the Configure method to set it
    /// </summary>
    public CacheType CacheType { get; private set; } = CacheType.InMemory;
    /// <summary>
    /// Contains the instance name (Read-Only) - Use the Configure or SetInstanceName method to set it
    /// </summary>
    public string? InstanceName { get; private set; } = "default";
    /// <summary>
    /// Contains the connection string (Read-Only) - Use the Configure method to set it
    /// </summary>
    public string? ConnectionString { get; private set; }
    /// <summary>
    /// Contains the health check setup (Read-Only) - Use ConfigureHealthCheck method to set it.
    /// </summary>
    public HealthCheckOptions? HealthCheck { get; private set; } = new(true, DefaultMaxErrorsAllowed, DefaultResetIntervalMinutes);
    /// <summary>
    /// Indicates if the cache is disabled - Use DisableCache method to set it.
    /// </summary>
    public bool Disabled { get; set; }
    /// <summary>
    /// Contains the default TTL for the cache which can be overwritten case by case - Use AddDefaultTtl method to set it.
    /// </summary>
    public DistributedCacheEntryOptions DefaultTtl { get; private set; } = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60),
        SlidingExpiration = TimeSpan.FromSeconds(30)
    };
    private const int DefaultMaxErrorsAllowed = 5;
    private const int DefaultResetIntervalMinutes = 5;
    
    /// <summary>
    /// Sets the basic configuration of the ICache interface
    /// </summary>
    /// <param name="cacheType">InMemory, Redis or SQL server</param>
    /// <param name="connectionString">ConnectionString for Redis or SQL server</param>
    /// <param name="instanceName">Instance name (Redis) or Table name (SQL server) </param>
    /// <returns>CacheOptions</returns>
    public CacheOptions Configure(CacheType cacheType, string? connectionString = null, string? instanceName = null)
    {
        CacheType = cacheType;
        ConnectionString = connectionString;
        InstanceName = instanceName;
        return this;
    }

    /// <summary>
    /// Sets the instance name of the cache
    /// </summary>
    /// <param name="instanceName">Instance name (Redis) or Table name (SQL server)</param>
    /// <returns>CacheOptions</returns>
    public CacheOptions SetInstanceName(string instanceName)
    {
        InstanceName = instanceName;
        return this;
    }
    
    /// <summary>
    /// Automatic healthcheck and recovery setup 
    /// </summary>
    /// <param name="enabled">Sets healthcheck enabled or disabled (enabled by default) </param>
    /// <param name="maxErrorsAllowed">Max number of errors before temporarily disable the cache</param>
    /// <param name="resetIntervalMinutes">Time in minutes to try to recover cache connectivity</param>
    /// <returns>CacheOptions</returns>
    public CacheOptions ConfigureHealthCheck(bool enabled, int maxErrorsAllowed = DefaultMaxErrorsAllowed, int resetIntervalMinutes = DefaultResetIntervalMinutes)
    {
        HealthCheck = new HealthCheckOptions(enabled, maxErrorsAllowed, resetIntervalMinutes);
        return this;
    }
    
    /// <summary>
    /// Disables the cache for cases in which you want to try the application without caching and without having to change the code 
    /// </summary>
    /// <param name="disable">True to disable the cache, false otherwise</param>
    /// <returns>CacheOptions</returns>
    public CacheOptions DisableCache(bool disable)
    {
        Disabled = disable;
        return this;
    }
    
    /// <summary>
    /// Add the default TTL to be used when no specific TTL is send
    /// </summary>
    /// <param name="absoluteExpirationRelativeToNow">Sets an absolute expiration time, relative to now</param>
    /// <param name="slidingExpiration">Sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed. This will not extend the entry lifetime beyond the absolute expiration</param>
    /// <returns></returns>
    public CacheOptions AddDefaultTtl(TimeSpan absoluteExpirationRelativeToNow, TimeSpan slidingExpiration)
    {
        DefaultTtl = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow,
            SlidingExpiration = slidingExpiration
        };
        
        return this;
    }
}

public enum CacheType 
{
    InMemory,
    Redis,
    SqlServer
}