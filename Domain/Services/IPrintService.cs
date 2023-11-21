using Contracts.Models;

namespace Domain.Services
{
    public interface IPrintService
    {
        void PrintForecasts(IEnumerable<WeatherResponse> forecasts);
    }
}