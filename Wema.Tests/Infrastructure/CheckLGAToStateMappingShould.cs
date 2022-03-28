using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wema.Tests.Infrastructure.Helpers;
using WemaCore.Helpers;
using Xunit;

namespace Wema.Tests.Infrastructure
{
	public class CheckLGAToStateMappingShould : MockCacheData
	{
		[Fact]
		public async Task CheckLGAToStateMapping_Maps_Correctly()
		{
			mockCache.Setup(x => x.GetFromCacheAsync())
			.ReturnsAsync(
				new List<StatesAndLGAs>
				{
					new StatesAndLGAs
					{
						State = new State
						{
							Name = "Oyo",
							Capital = "Ibadan",
							Id = 1,
							Locals = new List<Local>
							{
								new Local
								{
									Id = 1,
									Name = "Oluyole"
								},
								new Local
								{
									Id = 2,
									Name = "Orelope"
								}
							}
						}
					},
					new StatesAndLGAs
					{
						State = new State
						{
							Name = "Lagos",
							Capital = "Ikeja",
							Id = 1,
							Locals = new List<Local>
							{
								new Local
								{
									Id = 1,
									Name = "Badagri"
								},
								new Local
								{
									Id = 1,
									Name = "ikeja"
								}
							}
						}
					},

				}
			);

			var correctDataMap = await mappings.MappedCorrectly("oluyole", "oyo");
			Assert.True(correctDataMap);

			var incorrectDataMap = await mappings.MappedCorrectly("orelope", "lagos");
			Assert.False(incorrectDataMap);
		}
	}
}
