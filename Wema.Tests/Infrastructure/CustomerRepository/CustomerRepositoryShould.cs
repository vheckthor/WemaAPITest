using Moq;
using System.Threading.Tasks;
using Wema.Tests.Infrastructure.Helpers;
using WemaCore.DTOs.ApiResponse;
using WemaCore.Interfaces;
using WemaInfrastructure;
using WemaInfrastructure.Helpers;
using WemaInfrastructure.Repository.CustomerRepository;
using WemaCore.Entities;
using Xunit;

namespace Wema.Tests.Infrastructure.CustomerRepository
{
	public class CustomerRepositoryShould
	{
		[Fact]
		public async Task Return_Success_OnCall_UserExistsAsync_With_Existing_Data()
		{
			var _fileLoggerMock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("TestDB");

			await context.Customers.AddAsync(new Customer { Email = "ade@gmail.com", PhoneNumber = "23470321235356" });
			var saved = await context.SaveChangesAsync() > 0;
			Assert.True(saved);
			var command = new CustomerDataRepository(context, _fileLoggerMock.Object);

			var exc = await command.UserExistsAsync("ade@gmail.com", "23470321235356");

			Assert.True(exc);
		}

		[Fact]
		public async Task Return_Fail_OnCall_UserExistsAsync_With_NonExisting_Data()
		{
			var _fileLoggerMock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("MyDB");

			await context.Customers.AddAsync(new Customer { Email = "ade@gmail.com", PhoneNumber = "23470321235356" });
			var saved = await context.SaveChangesAsync() > 0;
			Assert.True(saved);
			var command = new CustomerDataRepository(context, _fileLoggerMock.Object);

			var exc = await command.UserExistsAsync("are@gmail.com", "23470321235359");

			Assert.False(exc);
		}

		[Fact]
		public async Task Return_Success_OnCreateUser_CreateCustomerAsync()
		{
			var _fileLoggerMock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("OwnDB");


			var command = new CustomerDataRepository(context, _fileLoggerMock.Object);

			var exc = await command.CreateCustomerAsync(new WemaCore.DTOs.ApiRequest.CreateCustomerRequest {
				Email = "ade@gmail.com", 
				Password = "null@bool", 
				PhoneNumber = "2347032193596",
				LGA = "Oyo",
				StateOfResidence = "Oyo"
			});
			Assert.IsType<bool>(exc);
			Assert.True(exc);
		}
		
		[Fact]
		public async Task Return_CorrectData_OnCall_GetCustomerAsync()
		{
			var _mock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("TestDB");

			await context.Customers.AddRangeAsync(
				new Customer { Email = "ade@gmail.com", PhoneNumber = "23470321235356" },
				new Customer { Email = "gre@gmail.com", PhoneNumber = "23407249575234"});
			var saved = await context.SaveChangesAsync() > 0;

			var command = new CustomerDataRepository(context, _mock.Object);
			var exc = await command.GetACustomerAsync("ade@gmail.com");

			Assert.NotNull(exc);
			Assert.IsType<GetCustomerResponse>(exc);
			Assert.True(exc.Email == "ade@gmail.com");
		}
	
		[Fact]
		public async Task Return_Success_OnCall_OnboardedCustomer()
		{
			var _mock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("MeDB");

			await context.Customers.AddRangeAsync(
				new Customer { Email = "ade@gmail.com", PhoneNumber = "23470321235356", Onboarded = true },
				new Customer { Email = "gre@gmail.com", PhoneNumber = "23407249575234" });
			var saved = await context.SaveChangesAsync() > 0;
			var command = new CustomerDataRepository(context, _mock.Object);
			var exc =await command.CustomerAlreadyOnboardedAsync("23470321235356");
			Assert.True(exc);
		}

		[Fact]
		public async Task Return_Failure_OnCall_OnboardedCustomer_With_IncorectData()
		{
			var _mock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("TestDB");

			await context.Customers.AddRangeAsync(
				new Customer { Email = "ade@gmail.com", PhoneNumber = "23470321235356", Onboarded = true },
				new Customer { Email = "gre@gmail.com", PhoneNumber = "23407249575234" });
			var saved = await context.SaveChangesAsync() > 0;
			var command = new CustomerDataRepository(context, _mock.Object);
			var exc = await command.CustomerAlreadyOnboardedAsync("23407249575234");
			Assert.False(exc);
		}
	
		[Fact]
		public async Task Return_Success_For_CustomerOnboarding_WithCorrectData()
		{
			var _mock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("TestDB");

			await context.Customers.AddRangeAsync(
				new Customer { Email = "ade@gmail.com", PhoneNumber = "23470321235446" },
				new Customer { Email = "gre@gmail.com", PhoneNumber = "23407249575223" });
			var saved = await context.SaveChangesAsync() > 0;
			var command = new CustomerDataRepository(context, _mock.Object);
			var exc = await command.OnboardCustomerAsync("23407249575223", true);
			Assert.True(exc);
		}

		[Fact]
		public async Task Return_Failure_For_CustomerOnboarding_WithInCorrectData()
		{
			var _mock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("TestDB");

			await context.Customers.AddRangeAsync(
				new Customer { Email = "ade@gmail.com", PhoneNumber = "23470321235446" },
				new Customer { Email = "gre@gmail.com", PhoneNumber = "23407249575223" });
			var saved = await context.SaveChangesAsync() > 0;
			var command = new CustomerDataRepository(context, _mock.Object);
			var exc = await command.OnboardCustomerAsync("23407249575323", true);
			Assert.False(exc);
		}

		[Fact]
		public async Task Return_Success_For_GetAllCustomer_And_Return_Only_Onboarded_Customers()
		{
			var _mock = new Mock<IFileLogger>();
			AppDbContext context = DbHelpers.InitContext("TestDB");

			await context.Customers.AddRangeAsync(
				new Customer { Email = "ade@gmail.com", PhoneNumber = "23470321235446", Onboarded = true },
				new Customer { Email = "are@gmail.com", PhoneNumber = "23470321235446", Onboarded = true },
				new Customer { Email = "arewe@gmail.com", PhoneNumber = "23470323235446", Onboarded = true },
				new Customer { Email = "andoe@gmail.com", PhoneNumber = "23470353235546" },
				new Customer { Email = "grower@gmail.com", PhoneNumber = "23470751235546" },
				new Customer { Email = "gre@gmail.com", PhoneNumber = "23407249575223" });
			var saved = await context.SaveChangesAsync() > 0;
			var command = new CustomerDataRepository(context, _mock.Object);
			var exc = await command.GetAllCustomersAsync(new WemaCore.Helpers.UserParams());
			Assert.NotNull(exc);
			Assert.IsType<PagedList<GetCustomerResponse>>(exc);
			Assert.Equal(3, exc.Count);
		}
	}
}
