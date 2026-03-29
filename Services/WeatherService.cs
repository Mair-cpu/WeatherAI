using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Mscc.GenerativeAI;
using WeatherAI.Models;

namespace WeatherAI.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IServiceScopeFactory _scopeFactory;

    public WeatherService(HttpClient httpClient, IConfiguration config, IServiceScopeFactory scopeFactory)
    {
        _httpClient = httpClient;
        _config = config;
        _scopeFactory = scopeFactory;
    }

    public async Task<WeatherRecord?> GetWeatherAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return null;

        var openWeatherKey = _config["WeatherSettings:OpenWeatherKey"];
        var geminiKey = _config["WeatherSettings:GeminiKey"];

        if (string.IsNullOrEmpty(openWeatherKey) || string.IsNullOrEmpty(geminiKey))
            return null; // Missing keys

        // 1. Fetch OpenWeather Data
        var weatherUrl = $"https://api.openweathermap.org/data/2.5/forecast?q={Uri.EscapeDataString(city)}&appid={openWeatherKey}&units=metric";
        var response = await _httpClient.GetAsync(weatherUrl);

        if (!response.IsSuccessStatusCode)
            return null;

        var weatherData = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
        if (weatherData?.list == null || !weatherData.list.Any())
            return null;

        // 2. Map Current Weather (using the first slot)
        var currentSlot = weatherData.list.First();
        var condition = currentSlot.weather?.FirstOrDefault()?.main ?? "Clear";
        
        var record = new WeatherRecord
        {
            City = weatherData.city?.name ?? city,
            TemperatureC = Math.Round(currentSlot.main?.temp ?? 0, 1),
            Condition = condition,
            Humidity = currentSlot.main?.humidity ?? 0,
            WindSpeedKmh = currentSlot.wind?.speed ?? 0,
            IconPath = condition switch
            {
                "Clear" or "Sunny" => "bi-sun-fill text-warning",
                "Clouds" or "Cloudy" => "bi-cloud-fill text-secondary",
                "Rain" => "bi-cloud-rain-fill text-primary",
                "Drizzle" => "bi-cloud-drizzle-fill text-info",
                "Thunderstorm" => "bi-cloud-lightning-rain-fill text-dark",
                "Snow" => "bi-snow text-info",
                _ => "bi-cloud-fill text-secondary"
            }
        };

        // 3. Perform LINQ Data Analysis over 5 days
        var avgTemp = Math.Round(weatherData.list.Average(x => x.main?.temp ?? 0), 1);
        var maxHumidity = weatherData.list.Max(x => x.main?.humidity ?? 0);
        var rainWarning = weatherData.list.Any(x => x.weather?.Any(w => w.main.Contains("Rain", StringComparison.OrdinalIgnoreCase)) ?? false);

        var analysis = new WeatherAnalysis
        {
            AverageTemperatureC = avgTemp,
            MaxHumidity = maxHumidity,
            RainWarning = rainWarning
        };

        // 4. Set analysis immediately without waiting for AI
        record.Analysis = analysis;

        // 5. Fire-and-forget AI tips so weather loads instantly
        _ = Task.Run(async () => {
            try {
                var (eng, urdu) = await GetAITipAsync(geminiKey, avgTemp, maxHumidity, rainWarning);
                analysis.GeminiTipEnglish = eng;
                analysis.GeminiTipUrdu = urdu;
            } catch { /* Silently ignore */ }
        });
        
        // 5. Persist to History Database
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WeatherAI.Services.WeatherDbContext>();
            db.SearchHistory.Add(new SearchHistory
            {
                CityName = record.City,
                Temp = record.TemperatureC,
                Condition = record.Condition,
                Timestamp = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
        catch (Exception)
        {
            // Fail silently if DB fails to write history
        }

        return record;
    }

    private async Task<(string English, string Urdu)> GetAITipAsync(string geminiKey, double avgTemp, int maxHumidity, bool rainWarning)
    {
        try
        {
            var googleAi = new GoogleAI(apiKey: geminiKey);
            var model = googleAi.GenerativeModel(model: "gemini-2.0-flash");

            var prompt = $"You are a smart Lifestyle Coach in Pakistan. Based on an average temp of {avgTemp}C and max humidity of {maxHumidity}%, give two short tips in English: one about what to wear or pack, and one about outdoor activities. Keep it friendly. Rain warning is {(rainWarning ? "true" : "false")}. Format strictly as:\nTip1: [tip]\nTip2: [tip]";

            var aiResult = await model.GenerateContent(prompt);
            var text = aiResult.Text;

            if (!string.IsNullOrEmpty(text))
            {
                var lines = text.Split('\n');
                var tip1 = lines.FirstOrDefault(l => l.StartsWith("Tip1:"))?.Replace("Tip1:", "").Trim();
                var tip2 = lines.FirstOrDefault(l => l.StartsWith("Tip2:"))?.Replace("Tip2:", "").Trim();
                
                return (
                    tip1 ?? "Stay prepared and have a great day!", 
                    tip2 ?? "Check the forecast before heading out!"
                );
            }
        }
        catch (Exception)
        {
            // Silently fall back to defaults
        }

        return ("Stay prepared to adapt to changing weather!", "Check local conditions before planning outdoor activities!");
    }

    public async Task<List<SearchHistory>> GetRecentSearchesAsync(int count = 5)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WeatherAI.Services.WeatherDbContext>();
            return await db.SearchHistory.OrderByDescending(x => x.Timestamp).Take(count).ToListAsync();
        }
        catch (Exception)
        {
            return new List<SearchHistory>();
        }
    }
}
