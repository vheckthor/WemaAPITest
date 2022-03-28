using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WemaCore.Interfaces;
using WemaInfrastructure;
using WemaInfrastructure.Helpers.Interfaces;

namespace WemaAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var datacache = services.GetRequiredService<ICacheStateAndLGA>();
					datacache.AddToCacheAsync().ConfigureAwait(true).GetAwaiter().GetResult();
					var context = services.GetRequiredService<AppDbContext>();
					context.Database.Migrate();

				}
				catch (Exception exception)
				{
					var logger = services.GetRequiredService<IFileLogger>();
					logger.LogError(exception + " An error occured during migrations", "ApplicationStart");

				}
			}
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
