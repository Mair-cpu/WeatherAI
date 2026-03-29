# 🌦️ WeatherAI - AI-Powered Weather Dashboard

A modern .NET 8 Blazor Web App featuring real-time weather data, AI-powered travel advice, and a stunning Glassmorphism UI.

## Features
- ☁️ **Real-Time Weather** — 5-day forecasts from OpenWeatherMap
- 🤖 **AI Weather Guardian** — Travel and safety advice powered by Google Gemini
- 🎨 **Dynamic Backgrounds** — Live wallpapers that change with weather conditions
- 💾 **SQLite Persistence** — Search history and chat messages saved locally
- 🪟 **Glassmorphism UI** — Beautiful dark glass-overlay design optimized for Retina displays

## Setup

### 1. Clone the repo
```bash
git clone https://github.com/YOUR_USERNAME/WeatherAI.git
cd WeatherAI
```

### 2. Configure API Keys
Edit `appsettings.json` and add your keys:
```json
{
  "WeatherSettings": {
    "OpenWeatherKey": "YOUR_OPENWEATHER_KEY",
    "GeminiKey": "YOUR_GEMINI_KEY"
  }
}
```
- Get an OpenWeatherMap key: https://openweathermap.org/api
- Get a Gemini key: https://aistudio.google.com/apikey

### 3. Run
```bash
dotnet run
```
Open http://localhost:5034

### 4. Docker (Optional)
```bash
docker build -t weatherai .
docker run -p 8080:8080 -v weatherai-data:/app/data \
  -e GEMINI_KEY=your_key \
  -e OPENWEATHER_KEY=your_key \
  weatherai
```

## Tech Stack
- .NET 8 Blazor Server (InteractiveServer)
- Entity Framework Core + SQLite
- Google Gemini 2.0 Flash (REST API)
- OpenWeatherMap API
- Bootstrap 5 + Custom Glassmorphism CSS

## License
MIT
