namespace WeatherAI.Models;

public class WeatherAnalysis
{
    public double AverageTemperatureC { get; set; }
    public int MaxHumidity { get; set; }
    public bool RainWarning { get; set; }
    public string GeminiTipEnglish { get; set; } = string.Empty;
    public string GeminiTipUrdu { get; set; } = string.Empty;
}
