using System;

namespace ShopApplication.Interfaces.Services;

public interface ICachingService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? exp);
    Task RemoveAsync(string key);
}