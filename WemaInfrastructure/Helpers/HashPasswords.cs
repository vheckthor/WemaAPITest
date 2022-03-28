using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WemaInfrastructure.Helpers
{
	public static class HashPasswords
	{
		public static void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordSalt)
		{
			using (var haser = new HMACSHA512())
			{
				passwordSalt = haser.Key;
				passwordhash = haser.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}
	}
}
