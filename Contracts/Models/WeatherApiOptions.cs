namespace Contracts.Models
{
    public class WeatherApiOptions
    {
        public string UserName { get; init; }

        public string Password { get; init; }

        public string BaseUrl { get; init; }

        public int CacheExpirationMinutes { get; init; }

    }
}