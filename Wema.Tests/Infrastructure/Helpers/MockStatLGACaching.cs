using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WemaCore.Interfaces;
using WemaInfrastructure.Helpers;

namespace Wema.Tests.Infrastructure.Helpers
{
	public class MockStatLGACaching
	{
		protected Mock<IMemoryCache> mockCache;
		protected Mock<IFileLogger> mockLogger;
		protected CacheStateAndLGA dataCache; 
		protected MockStatLGACaching()
		{
			mockCache = new Mock<IMemoryCache>();
			mockLogger = new Mock<IFileLogger>();
			dataCache = new CacheStateAndLGA(mockCache.Object, mockLogger.Object);
		}
	}
}
