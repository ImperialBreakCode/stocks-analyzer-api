using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MSSQLInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Infrastructure.MongoDbServices.WalletDatabaseServices;
using API.Settlement.Infrastructure.MSSQLServices.OutboxDatabaseServices;
using API.Settlement.Infrastructure.SQLiteServices.TransactionDatabaseServices;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Data.SQLite;



namespace API.Settlement.Extensions
{
	public static class DatabaseConfigurations
	{
		public static void AddDatabaseConfigurations(this IServiceCollection services, IConfiguration configuration)
		{
			AddSQLiteTransactionDatabaseConfiguration(services, configuration);
			AddMSSQLOutboxDatabaseConfiguration(services, configuration);
			AddMongoDBWalletDatabaseConfiguration(services,configuration);
		}

		private static void AddSQLiteTransactionDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("SQLiteTransactionConnection");
			if (!IsValidConnectionString(connectionString))
			{
				throw new Exception("Invalid SQLite Transaction database connection string!");
			}

			services.AddScoped(_ => new SQLiteConnection(connectionString));
			services.AddSingleton<ISQLiteTransactionDatabaseInitializer>(_ => new SQLiteTransactionDatabaseInitializer(connectionString));
		}

		private static void AddMSSQLOutboxDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("MSSQLOutboxConnection");
			if (!IsValidConnectionString(connectionString))
			{
				throw new Exception("Invalid MSSQL Outbox database connection string!");
			}

			services.AddScoped(_ => new SqlConnection(connectionString));
			services.AddSingleton<IMSSQLOutboxDatabaseInitializer>(_ => new MSSQLOutboxDatabaseInitializer(connectionString));
		}

		private static void AddMongoDBWalletDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<WalletDatabaseSettings>(configuration.GetSection("WalletDatabaseSettings"));
			services.AddSingleton<IWalletDatabaseSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<WalletDatabaseSettings>>().Value);
			services.AddSingleton<IMongoClient>(_ => new MongoClient(configuration.GetValue<string>("WalletDatabaseSettings:ConnectionString")));
		}

		private static bool IsValidConnectionString(string connectionString) => !string.IsNullOrWhiteSpace(connectionString);
	}
}
