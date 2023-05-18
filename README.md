# DistributedCache

![Build](https://img.shields.io/github/actions/workflow/status/Matozap/DistributedCache/build.yml?style=for-the-badge&logo=github&color=0D7EBF)
![Commits](https://img.shields.io/github/last-commit/Matozap/DistributedCache?style=for-the-badge&logo=github&color=0D7EBF)
![Package](https://img.shields.io/nuget/dt/DistributedCache?style=for-the-badge&logo=nuget&color=0D7EBF)


DistributedCache is an open source caching abstraction layer for .NET which supports Redis, SQL Server, InMemory, with automatic 
failure recovery, heath checks, automatic table generation (MSSQL), logging and also allows to clear all keys.


It simplifies cache usage by allowing developers to inject it into the application and use it anywhere
without having to worry about configuration for an specific infrastructure (InMemory, Redis, SQL server). 

It is as fast as a cache interface can be and also includes the feature of **CLEAR ALL** keys and **CLEAR ALL WITH PREFIX** which were both
missing from IDistributedCache.


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
        _ = _cache.SetCacheValueAsync(cacheKey, dataValue);

        return dataValue;
    }
}

```

#### Setting cache TTL

There are 2 ways to achieve that:

1. Using a default TTL (see configuration section)
2. Overriding the default TTL on a case-by-case basis like this:

```csharp

var ttl = new DistributedCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60),
    SlidingExpiration = TimeSpan.FromSeconds(30)
};
_ = _cache.SetCacheValueAsync(cacheKey, dataValue, ttl, cancellationToken);

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
        .ConfigureHealthCheck(enabled:true, maxErrorsAllowed:5, resetIntervalMinutes:2)
        .AddDefaultTtl(TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(30))
        .DisableCache(false);
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
