using API.Gateway.Helpers;
using API.Gateway.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddOptions();

builder.Services.AddServices();
builder.Services.InjectAuthentication(config);
builder.Services.ConfigureAppSettings(config);

builder.Host.ConfigureSerilog();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();	

app.UseHttpsRedirection();


app.UseRouting();

app.UseDatabaseInit();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<RequestSavingMiddlewere>();

app.UseWebSockets();

app.MapControllers();

app.Run();

