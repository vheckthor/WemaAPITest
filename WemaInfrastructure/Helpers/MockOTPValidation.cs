using Microsoft.Extensions.Caching.Memory;
using System;
using WemaCore.Interfaces;
using WemaInfrastructure.Helpers.Interfaces;

namespace WemaInfrastructure.Helpers
{
	public class MockOTPValidation : IMockOTPValidation
	{
		private readonly IFileLogger _logger;
		private readonly IMemoryCache _cache;

		public MockOTPValidation(IFileLogger logger, IMemoryCache cache)
		{
			_logger = logger;
			_cache = cache;
		}

		public int SendOTP(string phoneNumber)
		{
			_cache.CreateEntry(phoneNumber);
			var otp = OTPSetter(phoneNumber);
			_logger.LogInfo($"Otp generated successfully for {phoneNumber}", nameof(SendOTP));
			return otp;
		}
		
		public int ResendOTP(string phoneNumber)
		{
			var otp = OTPSetter(phoneNumber);
			_logger.LogInfo($"Otp re-generated successfully for {phoneNumber}", nameof(ResendOTP));
			return otp;
		}

		private int OTPSetter(string phoneNumber)
		{
			var generateOTP = new Random().Next(10000, 99999);
			var cacheExpiration = new MemoryCacheEntryOptions
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(10),
				Priority = CacheItemPriority.Normal,
			};
			var otp = _cache.Set(phoneNumber, generateOTP, cacheExpiration);
			return otp;
		}

		public bool ConfirmOTP(int otp, string phoneNumber)
		{
			if (_cache.TryGetValue(phoneNumber, out int correctOTP))
			{
				_cache.Remove(phoneNumber);
				return otp == correctOTP;
			};
			return false;
		}
	}
}
