using Microsoft.EntityFrameworkCore;
using dotenv.net;
using RESTful.Models;

namespace RESTful.Database
{
  public class WebAppDBContext : DbContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      DotEnv.Load();
      optionsBuilder.UseMySql(
        System.Environment.GetEnvironmentVariable("DB_CONNECTION"),
        new MySqlServerVersion(
          new Version(8, 0, 21)
        )
      );
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
  }
}
