﻿using API.Gateway.Domain.Interfaces.Helpers;
using Microsoft.Extensions.Options;

namespace API.Gateway.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void ConfigureWritable<T>(
			this IServiceCollection services,
			IConfigurationSection section,
			string file = "appsettings.json") where T : class, new()
		{
			services.Configure<T>(section);
			services.AddTransient<IWritableOptions<T>>(provider =>
			{
				var environment = provider.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
				var options = provider.GetService<IOptionsMonitor<T>>();
				return new WritableOptions<T>(environment, options, section.Key, file);
			});
		}
	}
}
