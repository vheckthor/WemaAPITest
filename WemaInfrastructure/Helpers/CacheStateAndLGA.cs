using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WemaCore.Constants;
using WemaCore.Helpers;
using WemaCore.Interfaces;
using WemaInfrastructure.Helpers.Interfaces;

namespace WemaInfrastructure.Helpers
{
	public class CacheStateAndLGA : ICacheStateAndLGA
	{
		private readonly IMemoryCache _cache;
		private readonly IFileLogger _logger;

		public CacheStateAndLGA(IMemoryCache cache, IFileLogger logger)
		{
			_cache = cache;
			_logger = logger;
		}

		public async Task<List<StatesAndLGAs>> AddToCacheAsync()
		{
			try
			{
				var cacheKey = AppConstants.CACHEKEYSTATEANDLGA;
				if (!_cache.TryGetValue(cacheKey, out List<StatesAndLGAs> dataResponse))
				{
					var data = await ParseStatesAndLGAFromJsonAsync();
					var cacheExpiration = new MemoryCacheEntryOptions
					{
						AbsoluteExpiration = DateTime.Now.AddYears(1),
						Priority = CacheItemPriority.Normal,
						SlidingExpiration = TimeSpan.FromMinutes(5000)
					};

					dataResponse = _cache.Set(cacheKey, data, cacheExpiration);
					_logger.LogInfo("Data saved to cache successfully", "AddToCacheAsync");
				}
				return dataResponse;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException?.Message, " AddToCacheAsync");
				return null;
			}
		}

		public async Task<List<StatesAndLGAs>> GetFromCacheAsync()
		{
			var cacheKey = AppConstants.CACHEKEYSTATEANDLGA;
			if(!_cache.TryGetValue(cacheKey, out List<StatesAndLGAs> dataResponse))
			{
				dataResponse = await AddToCacheAsync();
			}
			return dataResponse;
		}

		public async Task<List<StatesAndLGAs>> ParseStatesAndLGAFromJsonAsync()
		{
			var data = await File.ReadAllTextAsync("..\\WemaInfrastructure\\statesdata.json");
			var parsedData = JsonConvert.DeserializeObject<List<StatesAndLGAs>>(data);
			return  parsedData;
		}
	}
}
