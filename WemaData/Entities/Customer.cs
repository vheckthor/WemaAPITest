using System;
using System.ComponentModel.DataAnnotations;

namespace WemaCore.Entities
{
	public class Customer
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public Guid UserUniqueIdentity { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
		public string StateOfResidence { get; set; }
		public string LGA { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime CreatedDate { get; set; } 
		public bool Onboarded { get; set; }
	}
}
