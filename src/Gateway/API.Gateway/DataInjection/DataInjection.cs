using API.Gateway.DataInjection;
using API.Gateway.Domain.Interfaces.Helpers;
using API.Gateway.Extensions;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace API.Gateway.DependancyInjection
{
	public static class DataInjection
	{
		public static IServiceCollection AddData(this IServiceCollection services)
		{
			services
				.AddMyHttpClient()
				.AddServices()
				.AddHelpers()
				.AddRepositories()
				.AddFacoties()
				.AddDbHelpers()
				.AddMemoryCache();

			AddSwagger(services);


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
		public static void UseDatabaseInit(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var _databaseInit = scope.ServiceProvider.GetRequiredService<IDatabaseInit>();
				_databaseInit.Initialize();
			}
		}

		public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<MicroserviceHostsConfiguration>(
				configuration.GetSection("MicroserviceHosts"));

			services.ConfigureWritable<JwtOptionsConfiguration>(
				configuration.GetSection("Jwtoptions"));

			services.Configure<MongoDBConfiguration>(
				configuration.GetSection("MongoDB"));

			return services;
		}

		public static void ConfigureSerilog(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseSerilog((context, configuration) =>
				configuration
					.ReadFrom.Configuration(context.Configuration)
					.Enrich.FromLogContext()
			);
		}

		public static IServiceCollection AddSwagger(IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				var securityScheme = new OpenApiSecurityScheme
				{
					Name = "JWT Authentication",
					Description = "Enter JWT Bearer token **_only_**",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};
				options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{securityScheme, new string[] { }}
			});
			});

			return services;
		}
	}
}
