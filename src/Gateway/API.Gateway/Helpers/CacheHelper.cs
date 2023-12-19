using API.Gateway.Domain.Interfaces.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

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
			try
			{
				if (_memoryCache.TryGetValue(key, out T cachedData))
				{
					return cachedData;
				}

				return default;
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CacheHelper, Get method: {ex.Message}");
				return default;
			}
		}

		public void Set<T>(string key, T data, int AbsoluteExpInM, int SlidingExpInM)
		{
			try
			{
				var options = SetOptions(AbsoluteExpInM, SlidingExpInM);

				_memoryCache.Set(key, data, options);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CacheHelper, Set method: {ex.Message}");
			}
		}

		public void Remove(string key)
		{
			try
			{
				_memoryCache.Remove(key);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CacheHelper, Remove method: {ex.Message}");
			}
		}

		private MemoryCacheEntryOptions SetOptions(int AbsoluteExpInM, int SlidingExpInM)
		{
			var cacheOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(AbsoluteExpInM),
				SlidingExpiration = TimeSpan.FromMinutes(SlidingExpInM)
			};

			return cacheOptions;
		}
	}
}
