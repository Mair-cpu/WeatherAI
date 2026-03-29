namespace WeatherAI.Services;

public class WeatherAppState
{
    public string BackgroundClass { get; private set; } = "bg-default";
    public Models.WeatherRecord? CurrentWeather { get; private set; }

    public event Action? OnChange;

    public void SetBackground(string bgClass)
    {
        if (BackgroundClass != bgClass)
        {
            BackgroundClass = bgClass;
            OnChange?.Invoke();
        }
    }

    public void SetCurrentWeather(Models.WeatherRecord? weather)
    {
        CurrentWeather = weather;
        OnChange?.Invoke();
    }
}
