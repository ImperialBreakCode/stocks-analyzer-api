
using API.Settlement.Domain.Entities.Emails;
using System.Configuration;

namespace API.Settlement.Extensions
{
	public static class SettingsConfigurationsExtension
	{
		public static void AddSettingsConfigurations(this IServiceCollection services, IConfiguration configuration)
		{
			ConfigureSmtpSettings(services, configuration);
		}

		private static void ConfigureSmtpSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
		}
	}
}
