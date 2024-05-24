using Microsoft.AspNetCore.Mvc;
using SqlKata;
using SqlKata.Execution;
using RESTful.Models;
using RESTful.Database;

namespace RESTful.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
  private readonly ILogger<WeatherForecastController> _logger;
  private readonly IDatabaseConnection _db;
  private string GetWeatherSummary(int temperatureC)
  {
      if (temperatureC <= 0) return "Freezing";
      if (temperatureC <= 5) return "Bracing";
      if (temperatureC <= 10) return "Chilly";
      if (temperatureC <= 15) return "Cool";
      if (temperatureC <= 20) return "Mild";
      if (temperatureC <= 25) return "Warm";
      if (temperatureC <= 30) return "Balmy";
      if (temperatureC <= 35) return "Hot";
      if (temperatureC <= 40) return "Sweltering";
      return "Scorching";
  }

    private readonly ILogger<WeatherForecastController> _logger;
  public WeatherForecastController(ILogger<WeatherForecastController> logger, IDatabaseConnection databaseConnection)
  {
    _logger = logger;
    _db = databaseConnection;
  }

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
