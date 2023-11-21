using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class WeatherBackgroundService(
        IWeatherService weatherService,
        ILogger<WeatherBackgroundService> logger,
        IPrintService printService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cities = await weatherService.GetCitiesAsync();

                    var forecasts = await weatherService.GetForecastsAsync(cities);

                    printService.PrintForecasts(forecasts);

                    await weatherService.SaveForecastsAsync(forecasts);
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