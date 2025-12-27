using Microsoft.AspNetCore.Mvc;
using QcOnLocation.Domain;
using QcOnLocation.Services;

namespace QcOnLocation.Controllers;

/**
 * The Controller is responsible for endpoint routing.
 */

[ApiController]
[Route("weatherforecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly WeatherForecastService _weatherForecastService = new();

    [HttpGet]
    [Route("")]
    public IEnumerable<WeatherForecast> Get(bool celsius)
    {
            return _weatherForecastService.GetWeatherForecast(celsius);
            //test
    }
}