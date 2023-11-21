using Domain.Clients;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence.Repositories;

namespace Domain
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, string[] args)
        {
            services
                    .AddClients()
                    .AddServices(args);

            return services;
        }

        private static IServiceCollection AddClients(this IServiceCollection services)
        {
            services
                .AddHttpClient()
                .AddTransient<IWeatherApiClient, WeatherApiClient>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services, string[] args)
        {
            services
                .AddMemoryCache()
                .AddTransient<IWeatherService>(provider =>
                    new WeatherService(
                        provider.GetRequiredService<IWeatherApiClient>(),
                        provider.GetRequiredService<IWeatherRepository>(),
                        provider.GetRequiredService<IInputService>(),
                        provider.GetRequiredService<ILogger<WeatherService>>(),
                        args
                    )
                )
                .AddTransient<IInputService, InputService>()
                .AddTransient<IPrintService, PrintService>()
                .AddHostedService<WeatherBackgroundService>();

            return services;
        }
    }
}