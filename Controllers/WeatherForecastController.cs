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
    try
    {
      var result = _db.Connect()
        .Query("WeatherForecasts")
        .Get<WeatherForecast>();
      return Ok(result);
    }
    catch (Exception)
    {
      return Ok(new List<WeatherForecast>());
    }
  }

  [HttpGet("{id}")]
  public IActionResult Get(int id)
  {
    try
    {
      var result = _db.Connect()
        .Query("WeatherForecasts")
        .Where("id", id)
        .FirstOrDefault<WeatherForecast>();
      if (result == null)
        return NotFound();
      return Ok(result);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPost]
  public IActionResult Post([FromBody] WeatherForecast forecast)
  {
    var id = 0;
    try
    {
      id = _db.Connect()
        .Query("WeatherForecasts")
        .InsertGetId<int>(new
        {
          date = forecast.Date,
          temperatureC = forecast.TemperatureC,
          summary = forecast.Summary ?? GetWeatherSummary(forecast.TemperatureC)
        });
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }

    return Ok(_db.Connect()
      .Query("WeatherForecasts")
      .Where("id", id)
      .FirstOrDefault<WeatherForecast>()
    );
  }

  [HttpPatch("{id}")]
  public IActionResult Patch(int id, [FromBody] WeatherForecastPatch forecast)
  {
    var updates = new Dictionary<string, object>();
    if (forecast.Date != null)
      updates["Date"] = forecast.Date.Value;
    if (forecast.TemperatureC != null)
    {
      if (forecast.Summary == "auto")
        updates["Summary"] = GetWeatherSummary(forecast.TemperatureC.Value);
      updates["TemperatureC"] = forecast.TemperatureC.Value;
    }
    if (forecast.Summary != null && forecast.Summary != "auto")
      updates["Summary"] = forecast.Summary;

    if (updates.Count == 0)
    {
      return BadRequest("No valid fields supplied for update.");
    }

    try
    {
      var affected = _db.Connect()
        .Query("WeatherForecasts")
        .Where("id", id)
        .Update(updates);

      if (affected == 0)
        return NotFound();
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    };


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

    try
    {
      var affected = _db.Connect()
        .Query("WeatherForecasts")
        .Where("id", id)
        .Delete();
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }

    return Ok(result);
  }
}

