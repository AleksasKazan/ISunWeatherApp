using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services
                .AddRepositories()
                .AddDbContext<WeatherDbContext>(options =>
                    {
                        options.UseSqlite("Data Source=weather.db");
                    }, ServiceLifetime.Scoped)
                .AddDatabaseMigrations();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IWeatherRepository, WeatherRepository>();

            return services;
        }

        private static IServiceCollection AddDatabaseMigrations(this IServiceCollection services)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }

            return services;
        }
    }
}