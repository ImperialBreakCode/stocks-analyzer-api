using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Services;
using API.Settlement.Infrastructure.Services.Settlement_Services;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

var appsettingsJson = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHangfire(configuration => configuration
		.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
		.UseSimpleAssemblyNameTypeSerializer()
		.UseRecommendedSerializerSettings()
		.UseSqlServerStorage(appsettingsJson.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IHttpClient, MyHttpClient>();
builder.Services.AddScoped<ISettlementService, SettlementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseHangfireDashboard();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	endpoints.MapHangfireDashboard();
});

app.Run();