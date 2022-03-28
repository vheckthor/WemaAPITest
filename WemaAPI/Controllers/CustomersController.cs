using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WemaAPI.Extensions;
using WemaCore.Constants;
using WemaCore.DTOs.ApiRequest;
using WemaCore.Helpers;
using WemaCore.Interfaces;
using WemaInfrastructure.Helpers.Interfaces;
using WemaInfrastructure.Repository.CustomerRepository;

namespace WemaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly IFileLogger _logger;
		private readonly ICustomerDataRepository _customerRepo;
		private readonly ICheckLGAToStateMapping _toStateMapping;

		public CustomersController(IFileLogger logger, ICustomerDataRepository customerRepo, ICheckLGAToStateMapping toStateMapping)
		{
			_logger = logger;
			_customerRepo = customerRepo;
			_toStateMapping = toStateMapping;
		}

		[HttpGet("GetAllCustomers")]
		public async Task<IActionResult> GetAllCustomers([FromQuery] UserParams param)
		{
			var customers = await _customerRepo.GetAllCustomersAsync(param);
			if (customers == null)
			{
				return NotFound(AppConstants.DATANOTFOUND);
			}
			Response.AddPagination(customers.CurrentPage,
						  customers.PageSize,
						  customers.TotalCount,
						  customers.TotalPages);
			return Ok(customers);
		}

		[HttpGet("GetACustomer")]
		public async Task<IActionResult> GetACustomer(string email)
		{
			var customer = await _customerRepo.GetACustomerAsync(email);
			if (customer == null)
			{
				return NotFound(AppConstants.DATANOTFOUND);
			}
			return Ok(customer);
		}

		[HttpPost("AddCustomer")]
		public async Task<IActionResult> AddCustomer(CreateCustomerRequest customer)
		{
			if (await _customerRepo.UserExistsAsync(customer.Email.Trim().ToLower(), customer.PhoneNumber.Trim()))
			{
				return BadRequest(AppConstants.EMAILORPHONEALREADYEXISTS);
			}
			var correctLGAToStateMapping = await _toStateMapping.MappedCorrectly(customer.LGA, customer.StateOfResidence);
			if (!correctLGAToStateMapping)
			{
				return BadRequest(AppConstants.DATAENTRYINCORRECTFORLGAANDSTATE);
			}
			var created = await _customerRepo.CreateCustomerAsync(customer);
			if(!created)
			{
				return BadRequest(AppConstants.SAVEERROR);
			}
			return CreatedAtAction(nameof(GetACustomer),new { email = customer.Email});
		}
	}
}
