using Domain.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace isun
{
    public class WeatherBackgroundService(
        IWeatherService weatherService,
        ILogger<WeatherBackgroundService> logger,
        IPrintService printService,
        string[] args) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var forecasts = await weatherService.GetAndSaveForecastsAsync(args);
                    printService.PrintForecasts(forecasts);
                }
                catch (Exception ex)
                {
                    logger.LogError("An error occurred in Weather background service. {Message}", ex.Message);
                    return;
                }

                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }
    }
}