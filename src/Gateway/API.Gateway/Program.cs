using API.Gateway.Auth;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



public static class Program
{
	public static IConfiguration Configuration { get; private set; }

	public static void Main(string[] args)
	{
		Configuration = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.Build();

		var settingValue = Configuration["SectionName:SettingName"];


		var builder = WebApplication.CreateBuilder(args);



		builder.Services.AddControllers();
		builder.Services.AddScoped<IHttpClient, GwHttpClient>();
		builder.Services.AddScoped<IAccountService, AccountService>();
		builder.Services.AddScoped<IStocksService, StocksService>();

		builder.Services.AddScoped<TokenVerifier>();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(opts =>
			{
				byte[] signingKeyBytes = Encoding.UTF8
					.GetBytes(Configuration["Jwtoptions:SigningKey"]);

				opts.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["Jwtoptions:Issuer"],
					ValidAudience = Configuration["Jwtoptions:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwtoptions:SigningKey"]))
				};
			});
		builder.Services.AddAuthorization();

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