using API.StockAPI.Domain.Interfaces;
using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Domain.Utilities;
using API.StockAPI.Infrastructure.Configuration;
using API.StockAPI.Infrastructure.Context;
using API.StockAPI.Infrastructure.Helpers;
using API.StockAPI.Infrastructure.Interfaces;
using API.StockAPI.Infrastructure.Jobs;
using API.StockAPI.Infrastructure.Services;
using API.StockAPI.Services;
using Quartz;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>();
DatabaseHelper.EnsureDatabaseExists(builder.Configuration.GetConnectionString("Default"));

builder.Services.Configure<StockTypesConfig>(builder.Configuration.GetSection("StockTypesConfig"));

builder.Services.AddScoped<IDateCalculator, DateCalculator>();
builder.Services.AddScoped<IParametersAssigner, ParametersAssigner>();

builder.Services.AddScoped<IContextServices, ContextServices>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IExternalRequestService, ExternalRequestService>();
builder.Services.AddScoped<ITimedOutCallServices, TimedOutCallServices>();
builder.Services.AddScoped<IStockTypesConfigServices, StockTypesConfigServices>();

builder.Services.AddHttpClient<IExternalRequestService, ExternalRequestService>();

builder.Services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = JobKey.Create(nameof(ExecuteTimedOutAPICallsJob));
    options.AddJob<ExecuteTimedOutAPICallsJob>(jobKey).AddTrigger(
        trigger => trigger.ForJob(jobKey).WithCronSchedule("0 0 0/2 ? * * *"));
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Api-Sender", "API.StockAPI");
    await next.Invoke();
});

app.Run();
