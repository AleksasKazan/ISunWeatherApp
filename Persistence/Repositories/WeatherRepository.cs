using Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories
{
    public class WeatherRepository(
        WeatherDbContext dbContext,
        ILogger<WeatherRepository> logger) : IWeatherRepository
    {

        public async Task OverrideForecastsAsync(IEnumerable<WeatherResponse> forecasts)
        {
            try
            {
                using var transaction = dbContext.Database.BeginTransaction();
                try
                {
                    if (forecasts.Any())
                    {
                        dbContext.WeatherData.RemoveRange(await dbContext.WeatherData.ToListAsync());
                        await dbContext.SaveChangesAsync();

                        await dbContext.AddRangeAsync(forecasts);
                        var result = await dbContext.SaveChangesAsync();

                        transaction.Commit();

                        if (result > 0)
                        {
                            logger.LogInformation("Weather data saved successfully. New records: {Result}", result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.LogError("Error while saving weather data: {Message}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error while saving weather data: {Message}", ex.Message);
            }
        }
    }
}