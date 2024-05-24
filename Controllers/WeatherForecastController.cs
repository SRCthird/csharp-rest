using Microsoft.AspNetCore.Mvc;
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

  public WeatherForecastController(ILogger<WeatherForecastController> logger, IDatabaseConnection databaseConnection)
  {
    _logger = logger;
    _db = databaseConnection;
  }

  [HttpGet]
  public IActionResult Get()
  {
    var result = _db.Connect()
      .Query("WeatherForecasts")
      .Get<WeatherForecast>();
    return Ok(result);
  }

  [HttpGet("{id}")]
  public IActionResult Get(int id)
  {
    var result = _db.Connect()
      .Query("WeatherForecasts")
      .Where("id", id)
      .FirstOrDefault<WeatherForecast>();
    if (result == null)
      return NotFound();
    return Ok(result);
  }

  [HttpPost]
  public IActionResult Post([FromBody] WeatherForecast forecast)
  {
    if (forecast.Summary == null)
    {
        forecast.Summary = GetWeatherSummary(forecast.TemperatureC);
    }

    var id = _db.Connect()
      .Query("WeatherForecasts")
      .InsertGetId<int>(new{
        date = forecast.Date,
        temperatureC = forecast.TemperatureC,
        summary = forecast.Summary
      });

    return Ok(_db.Connect()
      .Query("WeatherForecasts")
      .Where("id", id)
      .FirstOrDefault<WeatherForecast>()
    );
  }

  [HttpPatch("{id}")]
  public IActionResult Patch(int id, [FromBody] WeatherForecastPatch forecast)
  {
    var db = _db.Connect();
    var updateQuery = db.Query("WeatherForecasts").Where("id", id);

    var updates = new Dictionary<string, object>();
    if (forecast.Date != null)
      updates["date"] = forecast.Date.Value;
    if (forecast.TemperatureC != null)
      updates["temperatureC"] = forecast.TemperatureC.Value;
    if (forecast.Summary != null)
      updates["summary"] = forecast.Summary;

    if (updates.Count == 0)
    {
      return BadRequest("No valid fields supplied for update.");
    }

    var affected = updateQuery.Update(updates);

    if (affected == 0)
      return NotFound();

    return Ok(_db.Connect()
      .Query("WeatherForecasts")
      .Where("id", id)
      .FirstOrDefault<WeatherForecast>()
    );
  }

  [HttpDelete("{id}")]
  public IActionResult Delete(int id)
  {
    var result = _db.Connect()
      .Query("WeatherForecasts")
      .Where("id", id)
      .FirstOrDefault<WeatherForecast>();

    if (result == null)
      return NotFound();

    var affected = _db.Connect()
      .Query("WeatherForecasts")
      .Where("id", id)
      .Delete();

    return Ok(result);
  }
}

