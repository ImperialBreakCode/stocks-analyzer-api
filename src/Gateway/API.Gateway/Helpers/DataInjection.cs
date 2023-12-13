using API.Gateway.Domain.Entities.Factories;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Extensions;
using API.Gateway.Infrastructure.Contexts;
using API.Gateway.Infrastructure.Helpers;
using API.Gateway.Infrastructure.Init;
using API.Gateway.Infrastructure.Provider;
using API.Gateway.Infrastructure.Services.MongoDB;
using API.Gateway.Services;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace API.Gateway.Helpers
{
    public static class DataInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
			services.AddHttpClient();

			services.AddSingleton<IHttpClient, GwHttpClient>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IStockInfoService, StockInfoService>();
            services.AddTransient<IAnalyzerService, AnalyzerService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<IStockService, StockService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IJwtTokenParser, JwtTokenParser>();
            services.AddSingleton<SQLiteContext>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IDatabaseInit, SQLiteDBInit>();
            services.AddTransient<IWebSocketService, WebSocketService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRequestServiceHelper, RequestServiceHelper>();
            services.AddTransient<ResponseDTOFactory>();
			services.AddTransient<ICacheHelper, CacheHelper>();

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
        public static void UseDatabaseInit(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var _databaseInit = scope.ServiceProvider.GetRequiredService<IDatabaseInit>();
                _databaseInit.Initialize();
            }
        }

		public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<MicroserviceHostsConfiguration>(
				configuration.GetSection("MicroserviceHosts"));

			services.ConfigureWritable<JwtOptionsConfiguration>(
				configuration.GetSection("Jwtoptions"));

			services.Configure<MongoDBConfiguration>(
				configuration.GetSection("MongoDB"));
		}

		public static void ConfigureSerilog(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseSerilog((context, configuration) =>
				configuration
					.ReadFrom.Configuration(context.Configuration)
					.Enrich.FromLogContext()
			);
		}

		public static void AddSwaggerGen(IServiceCollection services)
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
		}
	}
}
