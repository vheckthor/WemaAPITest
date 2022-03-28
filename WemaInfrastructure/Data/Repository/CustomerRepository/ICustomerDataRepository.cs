using System.Threading.Tasks;
using WemaCore.DTOs.ApiRequest;
using WemaCore.DTOs.ApiResponse;
using WemaCore.Helpers;
using WemaInfrastructure.Helpers;

namespace WemaInfrastructure.Repository.CustomerRepository
{
	public interface ICustomerDataRepository
	{
		Task<bool> CreateCustomerAsync(CreateCustomerRequest request);
		Task<PagedList<GetCustomerResponse>> GetAllCustomersAsync(UserParams param);
		Task<GetCustomerResponse> GetACustomerAsync(string email);
		Task<bool> UserExistsAsync(string username, string phoneNumber);
		Task<bool> OnboardCustomerAsync(string phonenumber, bool otpValid);
		Task<bool> CustomerAlreadyOnboardedAsync(string phoneNumber);
	}
}