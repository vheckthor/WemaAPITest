using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wema.Tests.Infrastructure.Helpers;
using WemaCore.Constants;
using WemaCore.Helpers;
using Xunit;

namespace Wema.Tests.Infrastructure
{
	public class CacheStateAndLGAShould :  MockStatLGACaching
	{
		[Fact]
		public async Task GetFromCacheAsync_Should_Get_Data_From_Cache()
		{
			var mockCacheEntry = new Mock<ICacheEntry>();
			string? keyPayload = null;
			mockCache.Setup(mc => mc.CreateEntry(It.IsAny<string>()))
				.Callback((object k) => keyPayload = (string) k)
				.Returns(mockCacheEntry.Object);

			object? valuePayload = null;
			mockCacheEntry
				.SetupSet(mce => mce.Value = It.IsAny<object>())
				.Callback<object>(v => valuePayload = v);

			TimeSpan? expirationPayload = null;
			mockCacheEntry
				.SetupSet(mce => mce.AbsoluteExpirationRelativeToNow = It.IsAny<TimeSpan?>())
				.Callback<TimeSpan?>(dto => expirationPayload = dto);
			var exc = await dataCache.GetFromCacheAsync();
			Assert.Null(exc);
		}
	}
}
