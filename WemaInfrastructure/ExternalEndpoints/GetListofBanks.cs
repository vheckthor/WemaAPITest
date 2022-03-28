using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WemaCore.Constants;
using WemaCore.Entities;
using WemaCore.Interfaces;

namespace WemaInfrastructure.ExternalEndpoints
{
	public class GetListofBanks : IGetListofBanks
	{
		private readonly IMemoryCache _cache;
		private readonly IConfiguration _config;
		private readonly IFileLogger _logger;
		static readonly HttpClient client = new HttpClient();
		public GetListofBanks(IMemoryCache cache, IConfiguration config, IFileLogger logger)
		{
			_cache = cache;
			_config = config;
			_logger = logger;
		}

		private async Task<ListOfBanks> ParseGetAllBanksApiResponse()
		{
			var response = await CallApi();
			if (!response.succeed)
			{
				return null;
			}
			var parsedData = JsonConvert.DeserializeObject<ListOfBanks>(response.data);
			return parsedData;
		}

		private async Task<(bool succeed, string data)> CallApi()
		{
			string subKey = Environment.GetEnvironmentVariable("subscriptionKey", EnvironmentVariableTarget.Machine);
			string url = _config.GetSection("GetBanksUrl").Value;
			client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);
			var response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				return (true, data);
			}
			return (false, "call failed");
		}

		private async Task<ListOfBanks> CacheApiResponse()
		{
			try
			{
				var cacheKey = AppConstants.CACHEKEYLISTOFBANKS;
				if (!_cache.TryGetValue(cacheKey, out ListOfBanks dataResponse))
				{
					var data = await ParseGetAllBanksApiResponse();
					var cacheExpiration = new MemoryCacheEntryOptions
					{
						AbsoluteExpiration = DateTime.Now.AddMonths(1),
						Priority = CacheItemPriority.Normal,
						SlidingExpiration = TimeSpan.FromMinutes(5000)
					};

					dataResponse = _cache.Set(cacheKey, data, cacheExpiration);
					_logger.LogInfo("Data saved to cache successfully", nameof(CacheApiResponse));
				}
				return dataResponse;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException?.Message, " AddToCacheAsync");
				return null;
			}
		}

		public async Task<ListOfBanks> GetBankListAsync()
		{
			var cacheKey = AppConstants.CACHEKEYSTATEANDLGA;
			if (!_cache.TryGetValue(cacheKey, out ListOfBanks dataResponse))
			{
				dataResponse = await CacheApiResponse();
			}
			return dataResponse;
		}

	}
}
