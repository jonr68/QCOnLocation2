using QcOnLocation.Domain;
using QcOnLocation.Repository;

namespace QcOnLocation.Services;

/**
 * The Services is where business logic lives. It is middleware between the controller and the Controller and Repository
 * That isolates the responsibility of applying any logic or changes to the data.   
 */

public class WeatherForecastService
{
    private readonly WeatherForecastRepository _weatherForecastRepository = new();

    public IEnumerable<WeatherForecast> GetWeatherForecast(bool celsius)
    {
        var result = _weatherForecastRepository.GetWeatherForecast();

        if (!celsius) return result;

        var inCelsius = result.Select(forecast =>
        {
            forecast.Temperature = (forecast.Temperature - 32) * 5 / 9;
            return forecast;
        });

        return inCelsius;
    }
}