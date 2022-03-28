using System;
using System.Threading.Tasks;
using WemaInfrastructure.Helpers.Interfaces;

namespace WemaInfrastructure.Helpers
{
	public class CheckLGAToStateMapping : ICheckLGAToStateMapping
	{
		private readonly ICacheStateAndLGA _statelgaData;

		public CheckLGAToStateMapping(ICacheStateAndLGA statelgaData)
		{
			_statelgaData = statelgaData;
		}
		public async Task<bool> MappedCorrectly(string lga, string state)
		{
			var cacheData = await _statelgaData.GetFromCacheAsync();
			var mapping = cacheData.Find(_ => _.State.Name.Equals(state.Trim(), StringComparison.OrdinalIgnoreCase))?
				.State.Locals.Exists(_ => _.Name.Equals(lga.Trim(), StringComparison.OrdinalIgnoreCase));
			return mapping.HasValue ? mapping.Value : false;
		}
	}
}
