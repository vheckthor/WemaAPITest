using System.Threading.Tasks;

namespace WemaInfrastructure.Helpers.Interfaces
{
	public interface ICheckLGAToStateMapping
	{
		Task<bool> MappedCorrectly(string lga, string state);
	}
}