using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence
{
    public class WeatherDbContextFactory : IDesignTimeDbContextFactory<WeatherDbContext>
    {
        public WeatherDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();

            optionsBuilder.UseSqlite("Data Source=weather.db");

            return new WeatherDbContext(optionsBuilder.Options);
        }
    }
}