using Domain.Clients;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services
                    .AddClients()
                    .AddServices();

            return services;
        }

        private static IServiceCollection AddClients(this IServiceCollection services)
        {
            services
                .AddHttpClient()
                .AddTransient<IWeatherApiClient, WeatherApiClient>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddMemoryCache()
                .AddTransient<IWeatherService, WeatherService>()
                .AddTransient<IInputService, InputService>()
                .AddTransient<IPrintService, PrintService>();

            return services;
        }
    }
}