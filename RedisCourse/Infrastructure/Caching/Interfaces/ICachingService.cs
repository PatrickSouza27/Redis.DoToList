namespace RedisCourse.Infrastructure.Caching.Interfaces;

public interface ICachingService
{
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string value);
}