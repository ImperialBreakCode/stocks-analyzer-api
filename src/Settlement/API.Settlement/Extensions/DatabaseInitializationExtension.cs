using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MSSQLInterfaces.OutboxDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;

namespace API.Settlement.Extensions
{
	public static class DatabaseInitializationExtension
	{
		public static void UseDatabasesInitialization(this IApplicationBuilder app)
		{
			UseSQLiteTransactionDatabaseInitialization(app);
			UseMSSQLOutboxDatabaseInitialization(app);
		}
		private static void UseSQLiteTransactionDatabaseInitialization(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var _transactionDatabaseInitializer = scope.ServiceProvider.GetRequiredService<ISQLiteTransactionDatabaseInitializer>();
				_transactionDatabaseInitializer.Initialize();
			}
		}

		private static void UseMSSQLOutboxDatabaseInitialization(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var _outboxDatabaseInitializer = scope.ServiceProvider.GetRequiredService<IMSSQLOutboxDatabaseInitializer>();
				_outboxDatabaseInitializer.Initialize();
			}
		}

	}
}
