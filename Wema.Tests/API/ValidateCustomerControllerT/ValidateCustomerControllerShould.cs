using System;
using System.Collections.Generic;
using System.Linq;
using WemaCore.DTOs.ApiRequest;
using System.Threading.Tasks;
using Xunit;
using WemaCore.DTOs.ApiResponse;
using WemaCore.Constants;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Wema.Tests.API.ValidateCustomerControllerT
{
	public class ValidateCustomerControllerShould : BaseControllerTest
	{
		[Fact]
		public void RequestOTP_Sends_OTP_Correctly()
		{
			mockOTP.Setup(_ => _.SendOTP("234253546465")).Returns(12346);
			var response = validateCustomer.RequestOTP(new GetOTPRequest { PhoneNumber = "234253546465" });
			Assert.NotNull(response);
			var okObjectResult = Assert.IsType<OkObjectResult>(response);
			Assert.True(okObjectResult.StatusCode == 200);
			var more = Assert.IsType<OTPSentResponse>(okObjectResult.Value);
			Assert.Equal(AppConstants.OTPSENTSUCCESSFULLY, more.Message);
		}

		[Fact]
		public void ReSendOTP_ReSends_OTP_Correctly()
		{
			mockOTP.Setup(_ => _.ResendOTP("234253546465")).Returns(12346);
			var response = validateCustomer.ResendOTP(new GetOTPRequest { PhoneNumber = "234253546465" });
			Assert.NotNull(response);
			var okObjectResult = Assert.IsType<OkObjectResult>(response);
			Assert.True(okObjectResult.StatusCode == 200);
			var more = Assert.IsType<OTPSentResponse>(okObjectResult.Value);
			Assert.Equal(AppConstants.OTPRESENTSUCCESSFULLY, more.Message);
		}

		[Fact]
		public async Task ConfirmOTP_OTP_Correctly()
		{
			mockOTP.Setup(_ => _.ConfirmOTP(12345,"234253546465")).Returns(true);
			mockCustomerRepository.Setup(_ => _.OnboardCustomerAsync("234253546465", true)).ReturnsAsync(true);
			var response =await validateCustomer.ConfirmOTP(new ConfirmOTPRequest { OTP = 12345, Phonenumber = "234253546465" });
			Assert.NotNull(response);
			var okObjectResult = Assert.IsType<OkObjectResult>(response);
			Assert.True(okObjectResult.StatusCode == 200);
			var more = Assert.IsType<string>(okObjectResult.Value);
			Assert.Equal(AppConstants.CUSTOMERONBOARDINGSUCCESSFUL, more);
		}

		[Fact]
		public async Task Return_Error_OnWrong_OTP_Input_Or_Expired_OTP()
		{
			mockOTP.Setup(_ => _.ConfirmOTP(12345, "234253546465")).Returns(false);
			var response = await validateCustomer.ConfirmOTP(new ConfirmOTPRequest { OTP = 12345, Phonenumber = "234253546465" });
			Assert.NotNull(response);
			var objectResult = Assert.IsType<BadRequestObjectResult>(response);
			Assert.True(objectResult.StatusCode == 400);
		}

		[Fact]
		public async Task Return_Error_For_Onboarded_Customers_()
		{
			mockOTP.Setup(_ => _.ConfirmOTP(12345, "234253546465")).Returns(true);
			mockCustomerRepository.Setup(_ => _.CustomerAlreadyOnboardedAsync("234253546465")).ReturnsAsync(true);
			var response = await validateCustomer.ConfirmOTP(new ConfirmOTPRequest { OTP = 12345, Phonenumber = "234253546465" });
			Assert.NotNull(response);
			var objectResult = Assert.IsType<BadRequestObjectResult>(response);
			Assert.True(objectResult.StatusCode == 400);
		}
	}
}
