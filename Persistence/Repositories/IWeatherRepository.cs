using Contracts.Models;

namespace Persistence.Repositories
{
    public interface IWeatherRepository
    {
        Task OverrideForecastsAsync(IEnumerable<WeatherResponse> forecasts);
    }
}