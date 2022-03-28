using System.Security.Cryptography;
using System.Text;

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
