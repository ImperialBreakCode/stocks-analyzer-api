using API.Gateway.Extensions;
using API.Gateway.Middleware;
using API.Gateway.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(options =>
{
	// add JWT Authentication
	var securityScheme = new OpenApiSecurityScheme
	{
		Name = "JWT Authentication",
		Description = "Enter JWT Bearer token **_only_**",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		Reference = new OpenApiReference
		{
			Id = JwtBearerDefaults.AuthenticationScheme,
			Type = ReferenceType.SecurityScheme
		}
	};
	options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{securityScheme, new string[] { }}
	});
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseSerilogRequestLogging();	

app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();

