using Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
    {
        public DbSet<WeatherResponse> WeatherData { get; set; }
    }
}