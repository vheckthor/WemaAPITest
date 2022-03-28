using System.ComponentModel.DataAnnotations;

namespace WemaCore.DTOs.ApiRequest
{
	public class CreateCustomerRequest
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "You must specify password between 6 to 20 characters")]
		public string Password { get; set; }
		[Required]
		public string StateOfResidence { get; set; }
		[Required]
		public string LGA { get; set; }
		[Required]
		[Phone]
		public string PhoneNumber { get; set; }
	}
}
