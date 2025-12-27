namespace QcOnLocation.Domain;

/**
 * This is a modal it just a class for defining data  
 */
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int Temperature { get; set; }
    public string? Summary { get; set; }
}