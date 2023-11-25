using API.Gateway.Domain.Interfaces;
using API.Gateway.Infrastructure.Contexts;
using API.Gateway.Infrastructure.Models;
using API.Gateway.Infrastructure.Provider;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace API.Gateway.Infrastructure.Init
{
	public class DatabaseInit : IDatabaseInit
	{
		private readonly IHttpClient _httpClient;
		private readonly IEmailService _service;
		private readonly Context _context;
		public DatabaseInit(IHttpClient httpClient, IEmailService emailService, Context context)
		{
			_httpClient = httpClient;
			_service = emailService;
			_context = context;
		}

		public async Task Initialize()
		{
			if (!DatabaseExists())
			{
				await CreateTables();
			}
		}

		private async Task CreateTables()
		{
			try
			{
				using (var connection = _context.CreateConnection())
				{
					connection.Open();

					var createTableSql = "CREATE TABLE IF NOT EXISTS emails (email TEXT PRIMARY KEY NOT NULL);";
					connection.Execute(createTableSql);

					connection.Close();
				}
				await PopulateDB();
			}
			catch (Exception ex)
			{
				Log.Information($"Error creating database: {ex.Message}");
			}
		}
		private async Task PopulateDB()
		{
			try
			{
				int numberOfUsers = 10;
				string url = $"https://random-data-api.com/api/v2/users?size={numberOfUsers}";

				var res = await _httpClient.Get(url);
				ObjectResult response = (ObjectResult)res;

				var stringResult = response.Value.ToString();
				var users = JsonConvert.DeserializeObject<List<User>>(stringResult);

				foreach (var x in users)
				{
					Email email = new Email()
					{
						Mail = x.Email
					};

					_service.Create(email);

				}
			}
			catch (Exception ex)
			{
				Log.Information($"Error populating db: {ex.Message}");
			}
		}
		private bool DatabaseExists()
		{
			try
			{
				using (var connection = _context.CreateConnection())
				{
					connection.Open();

					var tableName = "emails";
					var tableExistsSql = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";

					var result = connection.Query<string>(tableExistsSql).FirstOrDefault();
					connection.Close();
					return result != null && result == tableName;
				}
			}
			catch (Exception)
			{
				return false;
			}

		}

	}
}

