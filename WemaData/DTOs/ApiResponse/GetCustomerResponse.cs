using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WemaCore.DTOs.ApiResponse
{
	public class GetCustomerResponse
	{
		public string Email { get; set; }
		public Guid UserUniqueIdentity { get; set; }
		public string StateOfResidence { get; set; }
		public string LGA { get; set; }
		public string PhoneNumber { get; set; }
	}
}
