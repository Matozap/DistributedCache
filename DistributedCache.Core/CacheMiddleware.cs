using DistributedCache.Core.Configuration;
using DistributedCache.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCache.Core;

public static class CacheMiddleware
{
    /// <summary>
    /// Adds ICache to the service collection and sets up connectivity depending in the selected cache type (InMemory, Redis or SQL server)
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options">Sets the options for the cache like the cached type, connection string and instance name</param>
    /// <returns>Service Collection</returns>
    /// <exception cref="ArgumentNullException">Connection string is required if the cache type is not InMemory</exception>
    public static IServiceCollection AddDistributedCache(this IServiceCollection services, Action<CacheOptions> options)
    {
        var cacheOptions = new CacheOptions();
        options.Invoke(cacheOptions);

        if (cacheOptions.CacheType != CacheType.InMemory && string.IsNullOrEmpty(cacheOptions.ConnectionString))
        {
            throw new ArgumentNullException(nameof(AddDistributedCache),"ConnectionString is required but was missing in cache registration");
        }
        
        switch (cacheOptions.CacheType)
        {
            case CacheType.Redis:
            {
                services.AddStackExchangeRedisCache(option =>
                {
                    option.Configuration = cacheOptions.ConnectionString;
                    option.InstanceName = cacheOptions.InstanceName;
                });
                break;
            }
            case CacheType.SqlServer:
            {
                services.AddDistributedSqlServerCache(option =>
                {
                    option.ConnectionString = cacheOptions.ConnectionString;
                    option.SchemaName = "dbo";
                    option.TableName = cacheOptions.InstanceName;
                });
                SqlServerTableHelper.CreateSqlCacheIfNotExists(cacheOptions.ConnectionString!, cacheOptions.InstanceName!);
                break;
            }
            case CacheType.InMemory:
            default:
            {
                services.AddDistributedMemoryCache();
                break;
            }
        }
        
        services.AddSingleton<ICache, Cache>();
        return services;
    }
}