using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WemaAPI.MiddleWare
{
	public class CustomResponseHeaderMiddleWare
	{
		readonly RequestDelegate _next;
		readonly IDictionary<string, string> _customHeaderMap;

		public CustomResponseHeaderMiddleWare(RequestDelegate next, IDictionary<string, string> customHeaderMap = default)
		{
			_next = next;
			_customHeaderMap = customHeaderMap;
		}

		public async Task Invoke(HttpContext context)
		{
			IHeaderDictionary headers = context.Response.Headers;

			if (_customHeaderMap != null)
			{
				foreach (var headerValuePair in _customHeaderMap)
				{
					context.Response.Headers[headerValuePair.Key] = headerValuePair.Value;
				}
			}

			await _next.Invoke(context);
		}
	}
}
