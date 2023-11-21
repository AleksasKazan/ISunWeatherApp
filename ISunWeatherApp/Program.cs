using Contracts.Models;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using Serilog;
using Serilog.Events;

var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Error)
    .WriteTo.File(
        path: Path.Combine(AppContext.BaseDirectory, "logs/log-.txt"),
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information,
        outputTemplate: "{Timestamp:HH:mm} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

using var host = CreateHostBuilder(args).Build();
host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddSerilog();
                });
                services.AddSingleton(loggerFactory);
                services.Configure<WeatherApiOptions>(hostContext.Configuration.GetSection("WeatherAPI"));
                services.AddDomain(args);
                services.AddPersistence();
            });