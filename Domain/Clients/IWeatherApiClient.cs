using Contracts.Models;

namespace Domain.Clients
{
    public interface IWeatherApiClient
    {
        Task<IEnumerable<string>> GetApiCitiesAsync();
        Task<WeatherResponse> GetApiForecastAsync(string city);
    }
}