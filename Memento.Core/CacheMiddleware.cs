using Memento.Core.Configuration;
using Memento.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Memento.Core;

public static class CacheMiddleware
{
    public static IServiceCollection AddMemento(this IServiceCollection services, Action<CacheOptions> options)
    {
        var cacheOptions = new CacheOptions();
        options.Invoke(cacheOptions);

        if (cacheOptions.CacheType != CacheType.InMemory && string.IsNullOrEmpty(cacheOptions.ConnectionString))
        {
            throw new ArgumentNullException(nameof(AddMemento),"ConnectionString is required but was missing in cache registration");
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
        
        services.AddSingleton(cacheOptions);
        services.AddSingleton<ICache, Cache>();
        return services;
    }
}