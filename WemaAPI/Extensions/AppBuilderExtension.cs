using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using WemaAPI.MiddleWare;

namespace WemaAPI.Extensions
{
	public static class ApplicationBuilderExtension
	{
		public static IApplicationBuilder UseCustomResponseHeaderMiddleware(this IApplicationBuilder appBuilder,
						IDictionary<string, string> customHeaderMap = default)
		{
			return appBuilder.UseMiddleware<CustomResponseHeaderMiddleWare>(customHeaderMap);
		}

		public static IApplicationBuilder ConfigureCors(this IApplicationBuilder appBuilder, IConfiguration config)
		{
			string allowedHosts = config["Appsettings:AllowedHosts"];

			if (string.IsNullOrEmpty(allowedHosts))
				return appBuilder.UseCors(x => x
							  .AllowAnyHeader()
							  .WithMethods(new string[4] { "POST", "PATCH", "HEAD", "OPTIONS" }));

			allowedHosts = allowedHosts.Trim();

			if (allowedHosts == "*")
				return appBuilder.UseCors(x => x
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
			else
			{
				string[] allowedHostArray;

				if (!allowedHosts.Contains(","))
					allowedHostArray = new string[1] { allowedHosts };
				else
					allowedHostArray = allowedHosts.Split(",", StringSplitOptions.RemoveEmptyEntries);

				return appBuilder.UseCors(x => x
						  .AllowAnyHeader()
						  .WithOrigins(allowedHostArray)
						  .WithMethods(new string[4] { "POST", "PATCH", "HEAD", "OPTIONS" }));
			}
		}
	}
}
