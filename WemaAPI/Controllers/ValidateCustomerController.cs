using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WemaCore.Constants;
using WemaCore.DTOs.ApiRequest;
using WemaCore.DTOs.ApiResponse;
using WemaInfrastructure.Helpers.Interfaces;
using WemaInfrastructure.Repository.CustomerRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WemaAPI.Controllers
{
	[Route("api/ValidateCustomer/")]
	[ApiController]
	public class ValidateCustomerController : ControllerBase
	{
		private readonly IMockOTPValidation _otpData;
		private readonly ICustomerDataRepository _customerData;

		public ValidateCustomerController(IMockOTPValidation otpData, ICustomerDataRepository customerData)
		{
			_otpData = otpData;
			_customerData = customerData;
		}

		[HttpPost("RequestOTP")]
		public IActionResult RequestOTP(GetOTPRequest request)
		{
			var otp = _otpData.SendOTP(request.PhoneNumber);
			return Ok(new OTPSentResponse { OTP = otp, Message = AppConstants.OTPSENTSUCCESSFULLY});
		}

		[HttpPost("ResendOTP")]
		public IActionResult ResendOTP(GetOTPRequest request)
		{
			var otp = _otpData.ResendOTP(request.PhoneNumber);
			return Ok(new OTPSentResponse{ OTP = otp, Message = AppConstants.OTPRESENTSUCCESSFULLY });
		}

		[HttpPost("ConfirmOTP")]
		public async Task<IActionResult> ConfirmOTP(ConfirmOTPRequest request)
		{
			var otpIsValid = _otpData.ConfirmOTP(request.OTP, request.Phonenumber);
			if (!otpIsValid)
			{
				return BadRequest(AppConstants.INVALIDOTP);
			}
			var customerAlreadyOnboared = await _customerData.CustomerAlreadyOnboarded(request.Phonenumber);
			if (customerAlreadyOnboared) 
			{
				return BadRequest(AppConstants.ERRORCUSTOMERALREADYONBOARDED);
			}
			var onboardCustomerSuccessFully =await _customerData.OnboardCustomerAsyc(request.Phonenumber, otpIsValid);
			if (!onboardCustomerSuccessFully)
			{
				return BadRequest(AppConstants.SAVEERROR);
			}
			return Ok(AppConstants.CUSTOMERONBOARDINGSUCCESSFUL);
		}
	}
}
