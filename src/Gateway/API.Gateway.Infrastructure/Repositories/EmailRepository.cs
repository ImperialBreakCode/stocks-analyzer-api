using API.Gateway.Domain.Entities.SQLiteEntities;
using API.Gateway.Domain.Interfaces.Services;
using API.Gateway.Infrastructure.Contexts;
using Dapper;
using Serilog;
using System.Data;

namespace API.Gateway.Infrastructure.Repositories
{
	public class EmailRepository : IEmailRepository
	{
		private readonly SQLiteContext _context;

		public EmailRepository(SQLiteContext context)
		{
			_context = context;
		}

		public async Task Create(Email email)
		{
			try
			{
				var query = "INSERT INTO emails (email) VALUES (@Mail);";

				var parameters = new DynamicParameters();

				parameters.Add("@Mail", email.Mail, DbType.String);

				using (var connection = _context.CreateConnection())
				{
					connection.Open();
					await connection.ExecuteAsync(query, parameters);
					connection.Close();
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error creating email: {ex.Message}");
			}
		}

		public bool Exists(string email)
		{
			try
			{
				using (var connection = _context.CreateConnection())
				{
					connection.Open();

					var query = "SELECT COUNT(1) FROM emails WHERE email = @Email";
					var count = connection.ExecuteScalar<int>(query, new { Email = email });

					connection.Close();
					return count > 0;
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error checking if email exists: {ex.Message}");
				return false;
			}
		}

		public async Task Delete(string email)
		{
			try
			{
				using (var connection = _context.CreateConnection())
				{
					connection.Open();

					var query = "DELETE FROM emails WHERE email = @Email";
					await connection.ExecuteAsync(query, new { Email = email });

					connection.Close();
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error deleting email: {ex.Message}");
			}
		}
	}
}
