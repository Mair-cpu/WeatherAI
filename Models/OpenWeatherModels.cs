namespace WeatherAI.Models;

public class OpenWeatherResponse
{
    public List<ForecastSlot>? list { get; set; }
    public CityInfo? city { get; set; }
}

public class ForecastSlot
{
    public MainInfo? main { get; set; }
    public List<WeatherInfo>? weather { get; set; }
    public WindInfo? wind { get; set; }
    public string? dt_txt { get; set; }
}

public class MainInfo
{
    public double temp { get; set; }
    public int humidity { get; set; }
}

public class WeatherInfo
{
    public string main { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public string icon { get; set; } = string.Empty;
}

public class WindInfo
{
    public double speed { get; set; }
}

public class CityInfo
{
    public string name { get; set; } = string.Empty;
}
