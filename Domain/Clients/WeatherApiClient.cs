using Contracts.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace Domain.Clients
{
    public class WeatherApiClient(IHttpClientFactory httpClientFactory,
        IOptions<WeatherApiOptions> options,
        ILogger<WeatherApiClient> logger,
        IMemoryCache memoryCache) : IWeatherApiClient
    {
        private readonly WeatherApiOptions _options = options.Value;

        private async Task<AuthorizationResponse> GetTokenAsync(bool useCache = true)
        {
            const string cacheKey = "WeatherApiToken";

            if (useCache && memoryCache.TryGetValue(cacheKey, out AuthorizationResponse? cachedToken) && cachedToken != null)
            {
                return cachedToken;
            }

            var authorizationRequest = new AuthorizationRequest
            {
                UserName = _options.UserName,
                Password = _options.Password
            };

            using var httpClient = httpClientFactory.CreateClient();

            try
            {
                var response = await httpClient.PostAsJsonAsync(_options.BaseUrl + "api/authorize", authorizationRequest);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadFromJsonAsync<AuthorizationResponse>();

                    memoryCache.Set(cacheKey, token, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(_options.CacheExpirationMinutes)
                    });

                    return token!;
                }

                var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();

                logger.LogError("Failed to get token. {ErrorTitle} {ErrorStatus}", error?.Title, error?.Status);

                throw new UnauthorizedAccessException($"Failed to get token. {error?.Title} {error?.Status}");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Failed to get token. {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetApiCitiesAsync()
        {
            const string cacheKey = "WeatherApiCities";

            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<string>? cachedCities) && cachedCities != null)
            {
                return cachedCities;
            }

            using var httpClient = httpClientFactory.CreateClient();

            var authResponse = await GetTokenAsync();

            var token = authResponse.Token;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await httpClient.GetAsync(_options.BaseUrl + "api/cities");

                if (response.IsSuccessStatusCode)
                {
                    var cities = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();

                    memoryCache.Set(cacheKey, cities, TimeSpan.FromMinutes(_options.CacheExpirationMinutes));

                    return cities!;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    authResponse = await GetTokenAsync(useCache: false);

                    token = authResponse.Token;

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    response = await httpClient.GetAsync(_options.BaseUrl + "api/cities");

                    if (response.IsSuccessStatusCode)
                    {
                        var cities = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();

                        memoryCache.Set(cacheKey, cities, TimeSpan.FromMinutes(_options.CacheExpirationMinutes));

                        return cities!;
                    }
                }

                var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();

                logger.LogError("Failed to get cities. {ErrorTitle} {ErrorStatus}", error?.Title,error?.Status);

                throw new Exception($"Failed to get cities. {error?.Title} {error?.Status}");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Failed to get cities. {Message}", ex.Message);
                throw;
            }
        }

        public async Task<WeatherResponse> GetApiForecastAsync(string city)
        {
            using var httpClient = httpClientFactory.CreateClient();

            var authResponse = await GetTokenAsync();

            var token = authResponse.Token;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await httpClient.GetAsync(_options.BaseUrl + $"api/weathers/{city}");

                if (response.IsSuccessStatusCode)
                {
                    var forecast = await response.Content.ReadFromJsonAsync<WeatherResponse>();

                    return forecast!;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    authResponse = await GetTokenAsync(useCache: false);

                    token = authResponse.Token;

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    response = await httpClient.GetAsync(_options.BaseUrl + $"api/weathers/{city}");

                    if (response.IsSuccessStatusCode)
                    {
                        var forecast = await response.Content.ReadFromJsonAsync<WeatherResponse>();

                        return forecast!;
                    }
                }

                var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();

                logger.LogError("Failed to get weather forecast for {City}. {ErrorTitle} {ErrorStatus}", city, error?.Title, error?.Status);

                throw new Exception($"Failed to get weather forecast for {city}. {error?.Title} {error?.Status}");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Failed to get weather forecast for {City}. {Message}", city, ex.Message);
                throw;
            }
        }
    }
}