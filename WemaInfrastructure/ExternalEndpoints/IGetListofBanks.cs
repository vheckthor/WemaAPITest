using System.Threading.Tasks;
using WemaCore.Entities;

namespace WemaInfrastructure.ExternalEndpoints
{
	public interface IGetListofBanks
	{
		Task<ListOfBanks> GetBankListAsync();
	}
}