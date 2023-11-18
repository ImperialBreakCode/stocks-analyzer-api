using API.Gateway.Domain.Interfaces;
using API.Gateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Gateway.Extensions
{
	public static class DataInjection
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddTransient<IHttpClient, GwHttpClient>();
			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IStockInfoService, StockInfoService>();
			services.AddTransient<IAnalyzerService, AnalyzerService>();
			services.AddTransient<IWalletService, WalletService>();
			services.AddTransient<IStockService, StockService>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IJwtTokenParser, JwtTokenParser>();

			services.AddHttpContextAccessor();
			services.AddMemoryCache();

			return services;
		}

		public static IServiceCollection InjectAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opts =>
				{
					byte[] signingKeyBytes = Encoding.UTF8
						.GetBytes(configuration["Jwtoptions:SigningKey"]);

					opts.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = configuration["Jwtoptions:Issuer"],
						ValidAudience = configuration["Jwtoptions:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwtoptions:SigningKey"]))
					};
				});

			services.AddAuthorization();

			return services;
		}
	}
}
