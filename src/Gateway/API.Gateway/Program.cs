using API.Gateway.Extensions;
using API.Gateway.Middleware;
using API.Gateway.Settings;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOptions();

builder.Services.Configure<MicroserviceHostsConfiguration>(
builder.Configuration.GetSection("MicroserviceHosts"));

builder.Services.AddServices().InjectAuthentication(builder.Configuration);

builder.Services.AddHttpClient();

builder.Host.UseSerilog((context, configuration) =>
	configuration
		.ReadFrom.Configuration(context.Configuration)
		.Enrich.FromLogContext()
);

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


app.UseSerilogRequestLogging();	

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();

