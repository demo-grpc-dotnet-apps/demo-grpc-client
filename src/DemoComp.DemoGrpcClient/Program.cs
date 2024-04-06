using DemoComp.DemoGrpcClient.Proto;
using DemoComp.DemoGrpcClient.Repositories;
using DemoComp.DemoGrpcClient.Services.V1.Greeter;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddSingleton<GreetRepository>();
builder.Services.AddSingleton<GetSayHelloService>();

// Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// gRPC Client
builder.Services.AddGrpcClient<Greeter.GreeterClient>(options =>
{
    options.Address = new UriBuilder
    {
        Scheme = builder.Configuration["DEMO_GRPC_API_SCHEME"],
        Host = builder.Configuration["DEMO_GRPC_API_HOST"],
        Port = int.Parse(builder.Configuration["DEMO_GRPC_API_PORT"] ?? throw new InvalidOperationException())
    }.Uri;

    var defaultMethodConfig = new MethodConfig
    {
        Names = { MethodName.Default },
        RetryPolicy = new RetryPolicy
        {
            MaxAttempts = 3,
            InitialBackoff = TimeSpan.FromSeconds(1),
            MaxBackoff = TimeSpan.FromSeconds(5),
            BackoffMultiplier = 1.5,
            RetryableStatusCodes = { StatusCode.Unavailable }
        }
    };
    options.ChannelOptionsActions.Add(channelOptions =>
    {
        channelOptions.ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } };
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHsts();

app.Run();

/// <summary>
///     For Integration Test.
///     <see href="https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#basic-tests-with-the-default-webapplicationfactory"/>
/// </summary>
public abstract partial class Program;
