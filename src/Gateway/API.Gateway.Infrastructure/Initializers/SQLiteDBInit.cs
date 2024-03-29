﻿using API.Gateway.Domain.DTOs;
using API.Gateway.Domain.Entities.SQLiteEntities;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Domain.Interfaces.Helpers;
using API.Gateway.Domain.Interfaces.Services;
using API.Gateway.Infrastructure.Contexts;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace API.Gateway.Infrastructure.Init
{
	public class SQLiteDBInit : IDatabaseInit
	{
		private readonly IHttpClient _httpClient;
		private readonly IEmailRepository _service;
		private readonly SQLiteContext _context;
		public SQLiteDBInit(IHttpClient httpClient, IEmailRepository emailService, SQLiteContext context)
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
				Log.Error($"Error creating database: {ex.Message}");
			}
		}
		private async Task PopulateDB()
		{
			try
			{
				int numberOfUsers = 100;
				string url = $"https://random-data-api.com/api/v2/users?size={numberOfUsers}";

				ObjectResult response = (ObjectResult)await _httpClient.Get(url);

				var stringResult = response.Value.ToString();
				var users = JsonConvert.DeserializeObject<List<UserDTO>>(stringResult);

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
				Log.Error($"Error populating db: {ex.Message}");
				await Task.Delay(1000);
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

