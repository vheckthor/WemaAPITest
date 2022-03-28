using Moq;
using WemaInfrastructure.Helpers;
using WemaInfrastructure.Helpers.Interfaces;

namespace Wema.Tests.Infrastructure.Helpers
{
	public class MockCacheData
	{
		protected Mock<ICacheStateAndLGA> mockCache;
		protected CheckLGAToStateMapping mappings;

		protected MockCacheData()
		{
			mockCache = new Mock<ICacheStateAndLGA>();
			mappings = new CheckLGAToStateMapping(mockCache.Object);
		}
	}
}
