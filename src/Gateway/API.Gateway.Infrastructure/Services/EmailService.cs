using API.Gateway.Infrastructure.Contexts;
using API.Gateway.Infrastructure.Models;
using Dapper;
using System.Data;

namespace API.Gateway.Infrastructure.Provider
{
	public class EmailService : IEmailService
	{
		private readonly Context _context;

		public EmailService(Context context)
		{
			_context = context;
		}

		public async Task<Email> Get()
		{
			var query = "SELECT rowid as Id, Email FROM Email;";

			using (var connection = _context.CreateConnection())
			{
				var email = await connection.QuerySingleOrDefaultAsync<Email>(query);
				return email;
			}
		}

		public async Task Create(Email email)
		{
			var query = "INSERT INTO emails (email) VALUES (@Mail);";

			var parameters = new DynamicParameters();

			parameters.Add("email", email.Mail, DbType.String);

			using (var connection = _context.CreateConnection())
			{
				Console.WriteLine("in");
				await connection.ExecuteAsync(query, parameters);
			}
		}
	}
}
