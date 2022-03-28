using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
