using Microsoft.Extensions.Caching.Memory;
using API.Gateway.Domain.Interfaces.Helpers;

namespace API.Gateway.Helpers
{
    public class CacheHelper : ICacheHelper
	{
		private readonly IMemoryCache _memoryCache;

		public CacheHelper(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public T? Get<T>(string key)
		{
			if (_memoryCache.TryGetValue(key, out T cachedData))
			{
				return cachedData;
			}

			return default;
		}

		public void Set<T>(string key, T data, MemoryCacheEntryOptions options)
		{
			_memoryCache.Set(key, data, options);
		}
	}
}
