# DistributedCache

DistributedCache is an open source caching abstraction layer for .NET which supports Redis, SQL Server, InMemory, with automatic
failure recovery, heath checks, automatic table generation (MSSQL), logging and also allows to clear all keys.


It simplifies cache usage by allowing developers to inject it into the application and use it anywhere
without having to worry about configuration for an specific infrastructure (InMemory, Redis, SQL server).

It is as fast as a cache interface can be and also includes the feature of **CLEAR ALL** keys which is missing from IDistributedCache.


------------------------------

### Usage

#### Setting/Getting values from cache

Just inject and use the ICache interface to store or obtain values from the cache like the example below:

```csharp

public class CountryManager
{
    private readonly ICache _cache;

    public CountryManager(ICache cache)
    {
        _cache = cache;
    }

    public async Task<List<CountryData>> GetAllCountriesAsync(CancellationToken cancellationToken)
    {
        var cacheKey = "some cache key";

        var cachedValue = await _cache.GetCacheValueAsync<List<CountryData>>(cacheKey, cancellationToken);
        if (cachedValue != null)
        {
            return cachedValue;
        }

        var dataValue = await GetAllCountriesFromRepositoryAsync();

        _ = _cache.SetCacheValueAsync(cacheKey, dataValue, cancellationToken);

        return dataValue;
    }
}

```


### Configuration

#### Scenario 1: InMemory Cache

This is the most basic, yet common, scenario in which the application uses a cache in memory to increase the performance of a single
instance of an application.

```csharp
services.AddDistributedCache(options =>
{
    options.Configure(CacheType.InMemory);
});
```

###

#### Scenario 2: Redis with automatic failure recovery

This configuration use redis infrastructure and will check if it is healthy, in case of repeated failures (`maxErrorsAllowed`) it will automatically
disable the cache and will check again in the interval given (`resetIntervalMinutes`).

```csharp
var connectionString = "some cache connection string";
services.AddDistributedCache(options =>
{
    options.Configure(CacheType.Redis, connectionString, "myApplicationCacheInstance")
        .ConfigureHealthCheck(enabled:true, maxErrorsAllowed:5, resetIntervalMinutes:2);
});
```

###

#### Scenario 3: SQL Server with automatic failure recovery

This configuration use redis infrastructure and will check if it is healthy, in case of repeated failures (`maxErrorsAllowed`) it will automatically
disable the cache and will check again in the interval given (`resetIntervalMinutes`).

```csharp
var connectionString = "some cache connection string";
services.AddDistributedCache(options =>
{
    options.Configure(CacheType.SqlServer, connectionString, "myApplicationCacheInstance")
        .ConfigureHealthCheck(enabled:true, maxErrorsAllowed:5, resetIntervalMinutes:2);
});
```


###

## Contributing

It is simple, as all things should be:

1. Clone it
2. Improve it
3. Make pull request

## Credits

- Initial development by [Slukad](https://github.com/Slukad)