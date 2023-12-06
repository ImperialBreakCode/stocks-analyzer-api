using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Infrastructure.Context;
using API.StockAPI.Infrastructure.Interfaces;
using API.StockAPI.Infrastructure.Jobs;
using API.StockAPI.Infrastructure.Services;
using API.StockAPI.Services;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IContextServices, ContextServices>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IExternalRequestService, ExternalRequestService>();
builder.Services.AddScoped<ITimedOutCallServices, TimedOutCallServices>();

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
