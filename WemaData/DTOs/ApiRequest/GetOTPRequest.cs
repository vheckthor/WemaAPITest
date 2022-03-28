using System.ComponentModel.DataAnnotations;

namespace WemaCore.DTOs.ApiRequest
{
	public class GetOTPRequest
	{
		[Required]
		[Phone]
		public string PhoneNumber { get; set; }
	}
}
