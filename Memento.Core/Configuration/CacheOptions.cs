namespace Memento.Core.Configuration;

public class CacheOptions
{
    public record HealthCheckOptions(bool Enabled, int MaxErrorsAllowed, int ResetIntervalMinutes);
    public CacheType CacheType { get; private set; }
    public string? InstanceName { get; private set; } = "default";
    public string? ConnectionString { get; private set; }
    public HealthCheckOptions? HealthCheck { get; private set; } = new(true, DefaultMaxErrorsAllowed, DefaultResetIntervalMinutes);
    public bool Disabled { get; set; }
    private const int DefaultMaxErrorsAllowed = 5;
    private const int DefaultResetIntervalMinutes = 5;

    public CacheOptions()
    {
    }
    
    public CacheOptions Configure(CacheType cacheType, string? connectionString = null, string? instanceName = null)
    {
        CacheType = cacheType;
        ConnectionString = connectionString;
        InstanceName = instanceName;
        return this;
    }

    public CacheOptions SetInstanceName(string instanceName)
    {
        InstanceName = instanceName;
        return this;
    }
    
    public CacheOptions ConfigureHealthCheck(bool enabled, int maxErrorsAllowed = DefaultMaxErrorsAllowed, int resetIntervalMinutes = DefaultResetIntervalMinutes)
    {
        HealthCheck = new HealthCheckOptions(enabled, maxErrorsAllowed, resetIntervalMinutes);
        return this;
    }
    
    public CacheOptions DisableCache(bool disable)
    {
        Disabled = disable;
        return this;
    }
}

public enum CacheType 
{
    InMemory,
    Redis,
    SqlServer
}