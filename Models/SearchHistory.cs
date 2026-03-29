namespace WeatherAI.Models;

public class SearchHistory
{
    public int Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public double Temp { get; set; }
    public string Condition { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
