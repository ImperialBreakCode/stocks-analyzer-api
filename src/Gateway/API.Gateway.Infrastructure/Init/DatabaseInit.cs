using API.Gateway.Domain.Interfaces;
using API.Gateway.Infrastructure.Models;
using API.Gateway.Infrastructure.Provider;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Gateway.Infrastructure.Init
{
	public class DatabaseInit : IDatabaseInit
	{
		private readonly IHttpClient _httpClient;
		private readonly IEmailService _service;
		public DatabaseInit(IHttpClient httpClient, IEmailService emailService)
		{
			_httpClient = httpClient;
			_service = emailService;
		}


		public async Task PopulateDB()
		{
			try
			{
				int numberOfUsers = 100;
				string url = $"https://random-data-api.com/api/v2/users?size={numberOfUsers}";

				ObjectResult response = (ObjectResult)await _httpClient.Get(url);

				var stringResult = response.Value.ToString();
				Console.WriteLine("Actual JSON Response:");
				Console.WriteLine(stringResult);
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
				Console.WriteLine(ex.Message);
			}
		}

	}

}

