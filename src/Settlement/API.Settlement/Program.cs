using API.Settlement.Extensions;
using Hangfire;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container
builder.Services.AddDatabaseConfigurations(configuration);
builder.Services.AddHangfireConfiguration(configuration);
builder.Services.AddSettingsConfigurations(configuration);
builder.Services.AddCustomServiceRegistrations();

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseCustomMiddlewares();

app.UseDatabasesInitialization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	endpoints.MapHangfireDashboard();
});

app.Run();