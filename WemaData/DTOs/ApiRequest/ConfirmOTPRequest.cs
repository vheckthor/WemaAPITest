using System.ComponentModel.DataAnnotations;

namespace WemaCore.DTOs.ApiRequest
{
	public class ConfirmOTPRequest
	{
		[Required]
		[Phone]
		public string Phonenumber { get; set; }
		[Required]
		public int OTP { get; set; }
	}
}
