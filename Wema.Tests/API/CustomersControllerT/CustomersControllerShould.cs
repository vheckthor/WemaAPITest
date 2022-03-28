using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WemaCore.Constants;
using WemaCore.DTOs.ApiRequest;
using WemaCore.DTOs.ApiResponse;
using WemaCore.Helpers;
using WemaInfrastructure.Helpers;
using Xunit;

namespace Wema.Tests.API.CustomersControllerT
{
	public class CustomersControllerShould : BaseControllerTest
	{
		[Fact]
		public async Task Throw_Error_On_Call_Of_GetAllCustomers()
		{
			var dataToPage = new List<GetCustomerResponse>
			{
				new GetCustomerResponse
				{
					Email = "ade@gmail.com",
					StateOfResidence = "oyo",
					LGA = "lagelu",
					PhoneNumber = "2345497564596",
					UserUniqueIdentity = Guid.NewGuid()
				},
				new GetCustomerResponse
				{
					Email = "ako@gmail.com",
					StateOfResidence = "lagos",
					LGA = "badagri",
					PhoneNumber = "2345497564596",
					UserUniqueIdentity = Guid.NewGuid()
				},
				new GetCustomerResponse
				{
					Email = "grner@gmail.com",
					StateOfResidence = "oyo",
					LGA = "Ibadan North",
					PhoneNumber = "2345497564596",
					UserUniqueIdentity = Guid.NewGuid()
				},
				new GetCustomerResponse
				{
					Email = "kotila@gmail.com",
					StateOfResidence = "oyo",
					LGA = "ibarapa east",
					PhoneNumber = "2345497564596",
					UserUniqueIdentity = Guid.NewGuid()
				},
			};
			var return_data = new PagedList<GetCustomerResponse>(dataToPage, dataToPage.Count, 1, 2);
			mockCustomerRepository.Setup(_ => _.GetAllCustomersAsync(It.IsAny<UserParams>()))
				.ReturnsAsync(return_data);
			await Assert.ThrowsAsync<NullReferenceException>(async () => await customersController.GetAllCustomers(new UserParams()
			));

		}

		[Fact]
		public async Task Return_A_Customer_When_Correct_Data_Is_Supplied()
		{
			mockCustomerRepository.Setup(_ => _.GetACustomerAsync(It.IsAny<string>()))
			.ReturnsAsync(
				new GetCustomerResponse
				{
					Email = "ade@gmail.com",
					StateOfResidence = "oyo",
					LGA = "lagelu",
					PhoneNumber = "2345497564596",
					UserUniqueIdentity = Guid.NewGuid()
				});
			var response = await customersController.GetACustomer("ade@gmail.com");
			Assert.NotNull(response);
			var okObjectResult = Assert.IsType<OkObjectResult>(response);
			Assert.True(okObjectResult.StatusCode == 200);
			var more = Assert.IsType<GetCustomerResponse>(okObjectResult.Value);
			Assert.Equal("ade@gmail.com", more.Email);
		}

		[Fact]
		public async Task Return_Error_When_No_Email_Is_Passed_To_GetCustomer()
		{
			IActionResult response = await customersController.GetACustomer("");

			var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);

			Assert.True(okObjectResult.StatusCode == 404);
			Assert.Contains(AppConstants.DATANOTFOUND, okObjectResult.Value.ToString());

		}
	
		[Fact]
		public async Task Return_Success_On_New_Customer_Added()
		{
			mockCustomerRepository.Setup(_ => _.UserExistsAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(false);
			mockMapping.Setup(_ => _.MappedCorrectly(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(true);
			mockCustomerRepository.Setup(_ => _.CreateCustomerAsync(It.IsAny<CreateCustomerRequest>()))
				.ReturnsAsync(
					true);
			var response = await customersController.AddCustomer(
				new CreateCustomerRequest 
				{
					Email = "ade@gmail.com",
					StateOfResidence = "oyo",
					LGA = "lagelu",
					PhoneNumber = "2345497564596",
					Password = "123@sakw"
				});
			Assert.NotNull(response);
			var okObjectResult = Assert.IsType<CreatedAtActionResult>(response);
			Assert.True(okObjectResult.StatusCode == 201);
		}

		[Fact]
		public async Task Return_Failure_On_New_Customer_Already_Existing()
		{
			mockCustomerRepository.Setup(_ => _.UserExistsAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(true);
			mockMapping.Setup(_ => _.MappedCorrectly(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(true);
			var response = await customersController.AddCustomer(
				new CreateCustomerRequest
				{
					Email = "ade@gmail.com",
					StateOfResidence = "oyo",
					LGA = "lagelu",
					PhoneNumber = "2345497564596",
					Password = "123@sakw"
				});
			Assert.NotNull(response);
			var objectResult = Assert.IsType<BadRequestObjectResult>(response);
			Assert.True(objectResult.StatusCode == 400);
		}

		[Fact]
		public async Task Return_Failure_On_New_Customer_LGA_And_State_Not_Mapping()
		{
			mockCustomerRepository.Setup(_ => _.UserExistsAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(true);
			mockMapping.Setup(_ => _.MappedCorrectly(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(false);
			var response = await customersController.AddCustomer(
				new CreateCustomerRequest
				{
					Email = "ade@gmail.com",
					StateOfResidence = "oyo",
					LGA = "lagelu",
					PhoneNumber = "2345497564596",
					Password = "123@sakw"
				});
			Assert.NotNull(response);
			var objectResult = Assert.IsType<BadRequestObjectResult>(response);
			Assert.True(objectResult.StatusCode == 400);
		}
	}
}
