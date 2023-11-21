using Contracts.Models;

namespace Domain.Services
{
    public class PrintService : IPrintService
    {
        public void PrintForecasts(IEnumerable<WeatherResponse> forecasts)
        {
            foreach (var forecast in forecasts)
            {
                Console.WriteLine("\r\n" + forecast.City);
                Console.WriteLine(" temperature: " + forecast.Temperature);
                Console.WriteLine(" precipitation: " + forecast.Precipitation);
                Console.WriteLine(" windSpeed: " + forecast.WindSpeed);
                Console.WriteLine(" summary: " + forecast.Summary);
            }
        }
    }
}