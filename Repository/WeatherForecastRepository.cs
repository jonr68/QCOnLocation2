using QcOnLocation.Domain;

namespace QcOnLocation.Repository;

/**
 * The Repository is responsible for retrieving data from a source like a database or another endpoint.
 */
public class WeatherForecastRepository
{
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return MockWeatherForecastData.GetMockWeatherForecastData();
    }
}

/**
 * This is just a source for mock data. 
 */
public static class MockWeatherForecastData
{
    public static WeatherForecast[] GetMockWeatherForecastData() =>
    [
        new() { Date = DateTime.Parse("2025/12/14"), Temperature = 41, Summary = "Hot" },
        new() { Date = DateTime.Parse("2025/12/15"), Temperature = -6, Summary = "Sweltering" },
        new() { Date = DateTime.Parse("2025/12/16"), Temperature = 45, Summary = "Balmy" },
        new() { Date = DateTime.Parse("2025/12/17"), Temperature = 16, Summary = "Mild" },
        new() { Date = DateTime.Parse("2025/12/18"), Temperature = 53, Summary = "Scorching" }
    ];
}