using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WemaCore.DTOs.ApiRequest;
using WemaCore.DTOs.ApiResponse;
using WemaCore.Entities;
using WemaCore.Helpers;
using WemaCore.Interfaces;
using WemaInfrastructure.Helpers;

namespace WemaInfrastructure.Repository.CustomerRepository
{
	public class CustomerDataRepository : ICustomerDataRepository
	{
		private readonly AppDbContext _context;
		private readonly IFileLogger _logger;

		public CustomerDataRepository(AppDbContext context, IFileLogger logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<bool> CreateCustomerAsync(CreateCustomerRequest request)
		{
			try
			{
				_logger.LogInfo(request.Email + " Creating Customer...", "CreateCustomerAsync");
				
				byte[] passwordhash, passwordSalt;
				HashPasswords.CreatePasswordHash(request.Password, out passwordhash, out passwordSalt);
				var uniqueId = Guid.NewGuid();
				var newCustomer = new Customer
				{
					CreatedDate = DateTime.Now,
					Email = request.Email,
					StateOfResidence = request.StateOfResidence,
					LGA = request.LGA,
					Onboarded = false,
					PasswordHash = passwordhash,
					PasswordSalt = passwordSalt,
					PhoneNumber = request.PhoneNumber,
					UserUniqueIdentity = uniqueId
				};

				await _context.Customers.AddAsync(newCustomer);
				var success = await _context.SaveChangesAsync() > 0;
				if (success)
				{
					_logger.LogInfo(request.Email + " Created Successfully ", " CreateCustomerAsync");
					return success;
				}
				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException?.Message, "  AuthenticationRepositoryRegisterAsync");
				return false;
			}
		}

		public async Task<bool> UserExistsAsync(string username, string phoneNumber)
		{

			try
			{
				if (await _context.Customers.AnyAsync(x => x.Email == username || x.PhoneNumber == phoneNumber))
				{
					_logger.LogInfo($"{username} or {phoneNumber}"+ " already exists", "UserExistsAsync");
					return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message + " " + "Inner Exception" + ex.InnerException.Message, "  CustomerDataRepositoryUserExistsAsync");
				throw ex;
			}


		}

		//this list returns only onboarded customers list
		public async Task<PagedList<GetCustomerResponse>> GetAllCustomersAsync(UserParams param)
		{
			var customersList = _context.Customers.Where(_ => _.Onboarded == true)
				.Select(
				_=> new GetCustomerResponse { 
					PhoneNumber =_.PhoneNumber, 
					Email = _.Email,
					StateOfResidence = _.StateOfResidence,
					LGA = _.LGA,
					UserUniqueIdentity = _.UserUniqueIdentity
				});

			return await PagedList<GetCustomerResponse>.CreateAsync(customersList, param.PageNumber, param.PageSize);
		}

		public async Task<bool> OnboardCustomerAsync(string phonenumber, bool otpValid)
		{
			var customer = await _context.Customers.FirstOrDefaultAsync(_ => _.PhoneNumber == phonenumber);
			if(customer == null)
			{
				return false;
			}
			customer.Onboarded = otpValid;
			_context.Customers.Update(customer);
			return await _context.SaveChangesAsync() > 0;
		}

		public Task<bool> CustomerAlreadyOnboardedAsync(string phoneNumber)
		{
			return _context.Customers.AnyAsync(_ => _.PhoneNumber == phoneNumber && _.Onboarded == true);
		}

		public async Task<GetCustomerResponse> GetACustomerAsync(string email)
		{
			var customer =await _context.Customers.FirstOrDefaultAsync(_ => _.Email.Trim().ToLower()==email.Trim().ToLower());
			if (customer != null)
			{
				return new GetCustomerResponse
				{
					Email = customer.Email,
					StateOfResidence = customer.StateOfResidence,
					LGA = customer.LGA,
					PhoneNumber = customer.PhoneNumber,
					UserUniqueIdentity = customer.UserUniqueIdentity
				};
			}
			return null;

		}
	}
}