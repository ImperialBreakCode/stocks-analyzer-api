using API.Gateway.Domain.Interfaces;
using API.Gateway.Extensions;
using API.Gateway.Services;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class Program
{
	public static IConfiguration Configuration { get; private set; }

	public static void Main(string[] args)
	{
		//TODO
		Configuration = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.Build();

		

		var builder = WebApplication.CreateBuilder(args);

		//TODO
		builder.Services.Configure<MicroserviceHostsConfiguration>(Configuration.GetSection("MicroserviceHosts"));

		builder.Services.AddControllers();

		builder.Services.AddServices().InjectAuthentication(Configuration);

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddHttpClient();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();

		app.Run();


	}
}