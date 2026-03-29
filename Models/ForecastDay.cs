namespace WeatherAI.Models;

public class ForecastDay
{
    public string DayName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public double TempAvg { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}
