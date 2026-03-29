namespace WeatherAI.Models;

public class WeatherRecord
{
    public string City { get; set; } = string.Empty;
    public double TemperatureC { get; set; }
    public string Condition { get; set; } = string.Empty;
    public int Humidity { get; set; }
    public double WindSpeedKmh { get; set; }
    public string IconPath { get; set; } = string.Empty;
    
    public WeatherAnalysis? Analysis { get; set; }
    public List<ForecastDay> ForecastDays { get; set; } = new();
}
