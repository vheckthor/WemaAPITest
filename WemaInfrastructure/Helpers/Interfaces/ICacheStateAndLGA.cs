using System.Collections.Generic;
using System.Threading.Tasks;
using WemaCore.Helpers;

namespace WemaInfrastructure.Helpers.Interfaces
{
	public interface ICacheStateAndLGA
	{
		Task<List<StatesAndLGAs>> AddToCacheAsync();
		Task<List<StatesAndLGAs>> GetFromCacheAsync();
	}
}