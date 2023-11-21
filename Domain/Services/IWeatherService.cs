using Contracts.Models;

namespace Domain.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<string>> GetCitiesAsync();
        Task<IEnumerable<WeatherResponse>> GetForecastsAsync(IEnumerable<string> cities);
        Task SaveForecastsAsync(IEnumerable<WeatherResponse> forecasts);
    }
}