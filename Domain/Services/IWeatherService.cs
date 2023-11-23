using Contracts.Models;

namespace Domain.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherResponse>> GetAndSaveForecastsAsync(string[] args);
    }
}