using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using WemaCore.Interfaces;
using WemaInfrastructure.Logger;
using WemaInfrastructure.Repository.CustomerRepository;
using WemaInfrastructure.Helpers.Interfaces;
using WemaInfrastructure.Helpers;

namespace WemaInfrastructure.Extensions
{
	public static class ServiceCollectionExtension
	{

		public static void ConfigureDBContextPool(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContextPool<AppDbContext>(optionBuilder =>
			{
				optionBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
				optionBuilder.EnableSensitiveDataLogging();
			});
		}

		public static void ConfigureDBContext(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<AppDbContext>(optionBuilder =>
			{
				optionBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

			});
		}


		public static void ResolveInfrastructureServices(this IServiceCollection services, IConfiguration config)
		{
			services.ConfigureDBContextPool(config);
			services.AddMemoryCache();
			services.AddScoped<ICustomerDataRepository, CustomerDataRepository>();
			services.AddSingleton<ICacheStateAndLGA, CacheStateAndLGA>();
			services.AddScoped<IMockOTPValidation, MockOTPValidation>();
			services.AddScoped<ICheckLGAToStateMapping, CheckLGAToStateMapping>();
			services.AddSingleton<IFileLogger, AppLoggerService>();

			services.AddHttpClient("BypassCertificateHttpClient").ConfigurePrimaryHttpMessageHandler(() =>
			{
				var clientHandler = new HttpClientHandler
				{
					ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
				};
				return clientHandler;
			});
			HttpClientFactoryServiceCollectionExtensions.AddHttpClient(services);

		}
	}
}
