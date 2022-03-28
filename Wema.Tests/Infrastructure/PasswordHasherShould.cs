using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WemaInfrastructure.Helpers;
using Xunit;

namespace Wema.Tests.Infrastructure
{
	public class PasswordHasherShould
	{
		[Fact]
		public void Hash_Password_And_Return_Salt_And_Hash()
		{
			HashPasswords.CreatePasswordHash("someRandy@123", out byte[] hash, out byte[] salt);
			Assert.NotNull(salt);
			Assert.NotNull(hash);
			Assert.NotEmpty(salt);
			Assert.NotEmpty(hash);
			Assert.IsType<byte[]>(hash);
			Assert.IsType<byte[]>(salt);
		}
	}
}
