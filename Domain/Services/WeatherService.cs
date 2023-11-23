using Domain.Clients;
using Contracts.Models;
using Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class WeatherService(
        IWeatherApiClient apiClient,
        IWeatherRepository weatherRepository,
        IInputService inputService,
        ILogger<WeatherService> logger) : IWeatherService
    {
        public async Task<IEnumerable<WeatherResponse>> GetAndSaveForecastsAsync(string[] args)
        {
            try
            {
                var cities = await GetCitiesAsync(args);
                var forecasts = await GetForecastsAsync(cities);
                await weatherRepository.OverrideForecastsAsync(forecasts);

                return forecasts;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to retrieve and save forecasts. {Message}", ex.Message);
                throw;
            }
        }

        private async Task<IEnumerable<string>> GetCitiesAsync(string[] args)
        {
            try
            {
                var apiCities = await apiClient.GetApiCitiesAsync();

                return inputService.GetCities(args, apiCities);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get cities. {Message}", ex.Message);
                throw;
            }
        }

        private async Task<IEnumerable<WeatherResponse>> GetForecastsAsync(IEnumerable<string> cities)
        {     
            var forecasts = new List<WeatherResponse>();

            foreach ( var city in cities )
            {
                try
                {
                    var forecast = await apiClient.GetApiForecastAsync(city);
                    forecasts.Add(forecast);
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to retrieve forecast for {City}. {Message}", city, ex.Message);
                }
            }

            return forecasts;
        }
    }
}