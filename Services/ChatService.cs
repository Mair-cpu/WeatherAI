using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WeatherAI.Models;

namespace WeatherAI.Services;

public class ChatService
{
    private readonly IConfiguration _config;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly HttpClient _httpClient;

    public ChatService(IConfiguration config, IServiceScopeFactory scopeFactory, HttpClient httpClient)
    {
        _config = config;
        _scopeFactory = scopeFactory;
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task<List<ChatMessage>> GetChatHistoryAsync()
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WeatherAI.Services.WeatherDbContext>();
            return await db.ChatMessages.OrderBy(x => x.Timestamp).ToListAsync();
        }
        catch (Exception)
        {
            return new List<ChatMessage>();
        }
    }

    public async Task<string> SendMessageAsync(string userMessage, WeatherRecord? currentWeather)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
            return string.Empty;

        var geminiKey = _config["WeatherSettings:GeminiKey"] 
            ?? Environment.GetEnvironmentVariable("GEMINI_KEY");

        if (string.IsNullOrEmpty(geminiKey))
        {
            Console.WriteLine("API Key Missing: Gemini API key not found in appsettings.json or environment variables.");
            return "Configuration error: The AI service API key is not configured. Please contact the administrator.";
        }

        Console.WriteLine($"Gemini API Key used: {geminiKey.Substring(0, 5)}...");

        string botResponse;
        try
        {
            string jsonContext = currentWeather != null && currentWeather.Analysis != null
                ? JsonSerializer.Serialize(currentWeather)
                : "{ \"Info\": \"No weather data loaded. Ask the user to search a city first.\" }";

            var prompt = $@"You are Weather Guardian, a professional assistant. Answer in English.
Provide travel and weather safety advice for Pakistan.
Never mention that you are an AI model.
Use the weather data below to give real, actionable advice about travel, packing, and driving conditions.
If the user greets you, respond warmly and ask where they are planning to travel.

[Weather Data]:
{jsonContext}

[User]:
{userMessage}";

            // Direct REST call to Gemini API (bypasses SDK v1beta routing issues)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={geminiKey}";
            
            var payload = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(responseJson);
                botResponse = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString() ?? "I wasn't able to generate a response. Please try again.";
            }
            else
            {
                Console.WriteLine($"Gemini API Error: {response.StatusCode} - {responseJson}");
                botResponse = "I'm having trouble reaching the weather satellite. Please try again in a moment.";
            }
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("ChatService: Request timed out after 15 seconds.");
            botResponse = "I'm having trouble reaching the weather satellite. Please try again in a moment.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ChatService Error: {ex.Message}");
            botResponse = "I'm having trouble reaching the weather satellite. Please try again in a moment.";
        }

        // Save to Database
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WeatherAI.Services.WeatherDbContext>();
            db.ChatMessages.Add(new ChatMessage
            {
                UserMessage = userMessage,
                BotResponse = botResponse,
                Timestamp = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
        catch (Exception)
        {
            // Silently handle DB write failures
        }

        return botResponse;
    }
}
