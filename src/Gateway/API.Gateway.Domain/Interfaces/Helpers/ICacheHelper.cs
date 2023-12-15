using Microsoft.Extensions.Caching.Memory;

namespace API.Gateway.Domain.Interfaces.Helpers
{
    public interface ICacheHelper
    {
        T? Get<T>(string key);
        void Set<T>(string key, T data, MemoryCacheEntryOptions options);
    }
}