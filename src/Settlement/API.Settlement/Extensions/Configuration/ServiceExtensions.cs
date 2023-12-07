using API.Settlement.Domain.Interfaces;
using Hangfire;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Data.SQLite;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.Data.SqlClient;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using API.Settlement.Domain.Interfaces.TransactionInterfaces.TransactionCompletionInterfaces;
using API.Settlement.Domain.Interfaces.TransactionInterfaces.OrderProcessingInterfaces;
using API.Settlement.Application.Mappings.Mappers;
using API.Settlement.Application.Mappings;
using API.Settlement.Application.Services.HTMLServices;
using API.Settlement.Domain.Interfaces.HTMLInterfaces;
using API.Settlement.Infrastructure.SQLiteServices.TransactionDatabaseServices;
using API.Settlement.Infrastructure.MongoDbServices.WalletDatabaseServices;
using API.Settlement.Middlewares;
using API.Settlement.Application.Services.HTTPServices;
using API.Settlement.Application.Services.RabbitMQServices;
using API.Settlement.Application.Services.TransactionServices.OrderProcessingServices;
using API.Settlement.Application.Services.TransactionServices.TransactionCompletionServices;
using API.Settlement.Application.Services;
using API.Settlement.Domain.Interfaces.CommissionInterfaces;
using API.Settlement.Domain.Interfaces.HangfireInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using API.Settlement.Domain.Interfaces.HTTPInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces;
using API.Settlement.Application.Helpers.DateTimeHelpers;
using API.Settlement.Application.Helpers.ConstantHelpers;
using API.Settlement.Application.Services.HangfireServices;
using API.Settlement.Application.Services.EmailServices;
using API.Settlement.Application.Helpers.CommissionHelpers;
using API.Settlement.Infrastructure.MSSQLServices.OutboxDatabaseServices;

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
		
		public static void AddCustomServices(this IServiceCollection services)
		{
			services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

			services.AddHttpClient();
			services.AddAutoMapper(typeof(MappingProfile).Assembly);

			services.AddScoped<IHTMLContentGenerator, HTMLContentGenerator>();
			services.AddScoped<IPDFGenerator, PDFGenerator>();

			services.AddTransient<IUserCommissionService, UserCommissionHelper>();
			services.AddTransient<IEmailService, EmailService>();

			services.AddScoped<IFailedTransactionRepository, FailedTransactionRepository>();
			services.AddScoped<ISuccessfulTransactionRepository, SuccessfulTransactionRepository>();
			services.AddScoped<ITransactionDatabaseContext, TransactionDatabaseContext>();

			services.AddScoped<IOutboxPendingMessageRepository, OutboxPendingMessageRepository>();
			services.AddScoped<IOutboxSuccessfullySentMessageRepository, OutboxSuccessfullySentMessageRepository>();
			services.AddScoped<IOutboxDatabaseContext, OutboxDatabaseContext>();

			services.AddTransient<IRabbitMQProducer, RabbitMQProducer>();
			services.AddTransient<IRabbitMQConsumer, RabbitMQConsumer>();
			services.AddTransient<IRabbitMQService, RabbitMQService>();

			services.AddTransient<ITransactionResponseHandlerService, TransactionResponseHandlerService>();
			services.AddTransient<IInfrastructureConstants, InfrastructureConstantsHelper>();

			services.AddScoped<ITransactionCompletionService, TransactionCompletionService>();
			services.AddScoped<IFailedTransactionCompletionService, FailedTransactionCompletionService>();
			services.AddTransient<IAvailabilityStockInfoResponseDTOMapper, AvailabilityStockInfoResponseDTOMapper>();
			services.AddTransient<IAvailabilityResponseDTOMapper, AvailabilityResponseDTOMapper>();
			services.AddTransient<IFinalizeTransactionResponseDTOMapper, FinalizeTransactionResponseDTOMapper>();
			services.AddTransient<ITransactionMapper, TransactionMapper>();
			services.AddTransient<IWalletMapper, WalletMapper>();
			services.AddTransient<IStockMapper, StockMapper>();
			services.AddTransient<INotifyingEmailMapper, NotifyingEmailMapper>();
			services.AddTransient<IFinalizingEmailMapper, FinalizingEmailMapper>();
			services.AddTransient<IOutboxPendingMessageMapper, OutboxPendingMessageMapper>();
			services.AddTransient<IOutboxSuccessfullySentMessageMapper, OutboxSuccessfullySentMessageMapper>();
			services.AddTransient<IMapperManagementWrapper, MapperManagementWrapper>();

			services.AddTransient<IDateTimeService, DateTimeHelper>();
			services.AddTransient<IBuyService, BuyService>();
			services.AddTransient<ISellService, SellService>();
			services.AddTransient<ITransactionProcessingService, TransactionProcessingService>();
			services.AddTransient<IHangfireJobService, HangfireJobService>();
			services.AddScoped<IHangfireService, HangfireService>();
			services.AddScoped<ISettlementService, SettlementService>();

			services.AddHostedService<RabbitMQWalletDeletionService>();
			services.AddScoped<IRabbitMQWalletDeletionService, RabbitMQWalletDeletionService>();
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