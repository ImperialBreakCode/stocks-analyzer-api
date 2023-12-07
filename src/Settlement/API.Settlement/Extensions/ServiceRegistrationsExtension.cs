using API.Settlement.Domain.Interfaces;
using Hangfire;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Data.SQLite;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
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
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MSSQLInterfaces.OutboxDatabaseInterfaces;

namespace API.Settlement.Extensions
{
    public static class ServiceRegistrationsExtension
    {

        public static void AddCustomServiceRegistrations(this IServiceCollection services)
        {
			// Register core services and dependencies
			services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
			services.AddHttpClient();
			services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
			services.AddScoped<IHTMLContentGenerator, HTMLContentGenerator>();
			services.AddScoped<IPDFGenerator, PDFGenerator>();
			services.AddTransient<IDateTimeService, DateTimeHelper>();
			services.AddTransient<IUserCommissionService, UserCommissionHelper>();
			services.AddTransient<IEmailService, EmailService>();

			// Register mappers
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

			// Register transaction-related services and repositories
			services.AddScoped<IFailedTransactionRepository, FailedTransactionRepository>();
			services.AddScoped<ISuccessfulTransactionRepository, SuccessfulTransactionRepository>();
			services.AddScoped<ITransactionUnitOfWork, TransactionUnitOfWork>();
			services.AddScoped<ITransactionDatabaseService, TransactionDatabaseService>();

			// Register outbox-related services and repositories
			services.AddScoped<IOutboxPendingMessageRepository, OutboxPendingMessageRepository>();
			services.AddScoped<IOutboxSuccessfullySentMessageRepository, OutboxSuccessfullySentMessageRepository>();
			services.AddScoped<IOutboxUnitOfWork, OutboxUnitOfWork>();

			// Register wallet-related services and repositories
			services.AddScoped<IWalletRepository, WalletRepository>();
			services.AddScoped<IWalletService, WalletService>();

			// Register RabbitMQ-related services
			services.AddHostedService<RabbitMQWalletDeletionService>();
			services.AddTransient<IRabbitMQProducer, RabbitMQProducer>();
			services.AddTransient<IRabbitMQConsumer, RabbitMQConsumer>();
			services.AddTransient<IRabbitMQService, RabbitMQService>();
			services.AddTransient<IRabbitMQWalletDeletionService, RabbitMQWalletDeletionService>();

			// Register transaction handling and mapping services
			services.AddTransient<ITransactionResponseHandlerService, TransactionResponseHandlerService>();
			services.AddTransient<IInfrastructureConstants, InfrastructureConstantsHelper>();

			// Register transaction-specific services
			services.AddScoped<ITransactionCompletionService, TransactionCompletionService>();
			services.AddScoped<IFailedTransactionCompletionService, FailedTransactionCompletionService>();

			// Register buy and sell services
			services.AddTransient<IBuyService, BuyService>();
			services.AddTransient<ISellService, SellService>();

			// Register transaction processing and related services
			services.AddTransient<ITransactionProcessingService, TransactionProcessingService>();
			services.AddTransient<IHangfireJobService, HangfireJobService>();
			services.AddScoped<IHangfireService, HangfireService>();

			// Register settlement service
			services.AddScoped<ISettlementService, SettlementService>();

		}





	}
}