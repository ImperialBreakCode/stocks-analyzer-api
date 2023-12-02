using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.MongoDb.WalletDb;
using API.Settlement.Infrastructure.Helpers.Constants;
using API.Settlement.Infrastructure.Services;
using API.Settlement.Infrastructure.Services.SQLiteServices;
using API.Settlement.Infrastructure.Services.MongoDbServices.WalletDbServices;
using Hangfire;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Data.SQLite;
using API.Settlement.Infrastructure.Services.EmailServices;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Infrastructure.Services.SQLiteServices.TransactionDatabase;
using API.Settlement.Extensions.Middlewares;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Infrastructure.Services.DatabasesServices.SQLiteServices.OutboxDatabaseServices;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using API.Settlement.Infrastructure.Services.MongoDbServices.WalletDatabasebServices;
using API.Settlement.Infrastructure.Services.RabbitMqServices;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using API.Settlement.Domain.DTOs.Response;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.Extensions.DependencyInjection;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces;
using Microsoft.Data.SqlClient;
using API.Settlement.Infrastructure.Services.RabbitMQServices;

namespace API.Settlement.Extensions.Configuration
{
	public static class ServiceExtensions
	{

		public static void AddSQLiteTransactionDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("SQLiteTransactionConnection");
			services.AddScoped(_ => new SQLiteConnection(connectionString));
			services.AddSingleton<ISQLiteTransactionDatabaseInitializer>(_ => new SQLiteTransactionDatabaseInitializer(connectionString));
		}

		public static void AddMSSQLOutboxDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("MSSQLOutboxConnection");
			services.AddScoped(_ => new SqlConnection(connectionString));
			services.AddSingleton<IMSSQLOutboxDatabaseInitializer>(_ => new MSSQLOutboxDatabaseInitializer(connectionString));
		}
		public static void UseSQLiteTransactionDatabaseInitialization(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var _databaseInitializer = scope.ServiceProvider.GetRequiredService<ISQLiteTransactionDatabaseInitializer>();
				_databaseInitializer.Initialize();
			}
		}

		public static void UseMSSQLOutboxDatabaseInitialization(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var _databaseInitializer = scope.ServiceProvider.GetRequiredService<IMSSQLOutboxDatabaseInitializer>();
				_databaseInitializer.Initialize();
			}
		}
		public static void AddCustomServices(this IServiceCollection services)
		{
			services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

			services.AddHttpClient();
			services.AddAutoMapper(typeof(Infrastructure.Mappings.MappingProfile).Assembly);

			services.AddScoped<IHTMLContentGenerator, HTMLContentGenerator>();
			services.AddScoped<IPDFGenerator, PDFGenerator>();

			services.AddScoped<IUserCommissionService, UserCommissionService>();
			services.AddTransient<IEmailService, EmailService>();

			services.AddScoped<IFailedTransactionRepository, FailedTransactionRepository>();
			services.AddScoped<ISuccessfulTransactionRepository, SuccessfulTransactionRepository>();
			services.AddScoped<ITransactionDatabaseContext, TransactionDatabaseContext>();

			services.AddScoped<IOutboxPendingMessageRepository, OutboxPendingMessageRepository>();
			services.AddScoped<IOutboxSuccessfullySentMessageRepository, OutboxSuccessfullySentMessageRepository>();
			services.AddScoped<IOutboxDatabaseContext, OutboxDatabaseContext>();

			services.AddTransient<IRabbitMQProducer, RabbitMQProducer>();
			services.AddScoped<IRabbitMQService, RabbitMQService>();

			services.AddTransient<ITransactionResponseHandlerService, TransactionResponseHandlerService>();
			services.AddTransient<IInfrastructureConstants, InfrastructureConstants>();
			services.AddTransient<ITransactionMapperService, TransactionMapperService>();
			services.AddTransient<IDateTimeService, DateTimeService>();
			services.AddTransient<IBuyService, BuyService>();
			services.AddTransient<ISellService, SellService>();
			services.AddTransient<ITransactionWrapper, TransactionWrapper>();
			services.AddTransient<IHangfireJobService, HangfireJobService>();
			services.AddScoped<IHangfireService, HangfireService>();
			services.AddScoped<ISettlementService, SettlementService>();
		}
		public static void AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddHangfire(config => config
			.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
			.UseSimpleAssemblyNameTypeSerializer()
			.UseRecommendedSerializerSettings()
			.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

			GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

			services.AddHangfireServer();
		}

		

		public static void AddMongoDBWalletDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<WalletDatabaseSettings>(
				configuration.GetSection(nameof(WalletDatabaseSettings)));

			services.AddSingleton<IWalletDatabaseSettings>(w =>
				w.GetRequiredService<IOptions<WalletDatabaseSettings>>().Value);

			services.AddSingleton<IMongoClient>(m =>
				new MongoClient(configuration.GetValue<string>("WalletDatabaseSettings:ConnectionString")));

			services.AddScoped<IWalletRepository, WalletRepository>();
			services.AddScoped<IWalletService, WalletService>();
		}

		public static void UseCustomMiddlewares(this IApplicationBuilder app)
		{
			app.UseMiddleware<IPFilterMiddleware>();
		}
	}
}