# 🌦️ WeatherAI — AI-Powered Weather Dashboard

A modern **.NET 8 Blazor Web App** featuring real-time weather data, an AI travel safety assistant (Weather Guardian), and a stunning Glassmorphism UI with dynamic backgrounds.

![.NET 8](https://img.shields.io/badge/.NET-8.0-purple) ![Blazor](https://img.shields.io/badge/Blazor-Server-blue) ![SQLite](https://img.shields.io/badge/Database-SQLite-green) ![Gemini AI](https://img.shields.io/badge/AI-Gemini%202.0-orange)

---
## 📺 Project Demo
Watch the WeatherAI dashboard in action, featuring the Glassmorphism UI and AI Chat integration.

[![WeatherAI Demo Video](https://img.youtube.com/vi/5BDDVXC2buQ/maxresdefault.jpg)](https://www.youtube.com/watch?v=5BDDVXC2buQ)

> **[Click here to watch the video on YouTube](https://www.youtube.com/watch?v=5BDDVXC2buQ)**

---

## ✨ Features

- ☁️ **Real-Time 5-Day Forecast** — Powered by OpenWeatherMap (3-hour intervals)
- 🤖 **AI Weather Guardian** — Travel & safety advice powered by Google Gemini 2.0 Flash
- 📊 **LINQ Data Analysis** — Average temperature, max humidity, and rain warnings
- 🎨 **Dynamic Backgrounds** — Wallpapers change based on weather (Sunny, Cloudy, Rainy, Snow, Haze, Thunder)
- 🪟 **Glassmorphism UI** — Dark glass-overlay design optimized for Retina/M1 displays
- 💾 **SQLite Persistence** — Search history & chat messages saved locally
- 📱 **Split-Pane Layout** — Weather dashboard (75%) + AI Chat sidebar (25%)
- 🐳 **Docker Ready** — Containerized deployment with persistent volume

---

## 📋 Prerequisites

Before running this project, make sure you have:

| Requirement | Version | Download |
|-------------|---------|----------|
| .NET SDK | 8.0+ | [Download](https://dotnet.microsoft.com/download/dotnet/8.0) |
| OpenWeatherMap API Key | Free tier | [Get Key](https://openweathermap.org/api) |
| Google Gemini API Key | Free tier | [Get Key](https://aistudio.google.com/apikey) |

---

## 🚀 Getting Started

### Step 1: Clone the Repository

```bash
git clone https://github.com/Mair-cpu/WeatherAI.git
cd WeatherAI
```

### Step 2: Add Your API Keys

Open `appsettings.json` and replace the placeholder values with your real keys:

```json
{
  "WeatherSettings": {
    "OpenWeatherKey": "YOUR_OPENWEATHER_KEY_HERE",
    "GeminiKey": "YOUR_GEMINI_KEY_HERE"
  }
}
```

#### How to get the keys:

**OpenWeatherMap Key:**
1. Go to [openweathermap.org](https://openweathermap.org/) and create a free account
2. Navigate to **API Keys** in your profile
3. Copy your default key or generate a new one
4. Paste it as the `OpenWeatherKey` value

**Google Gemini Key:**
1. Go to [aistudio.google.com/apikey](https://aistudio.google.com/apikey)
2. Click **"Create API Key"**
3. Select or create a Google Cloud project
4. Copy the generated key
5. Paste it as the `GeminiKey` value

### Step 3: Create the Data Directory

```bash
mkdir data
```

This is where the SQLite database (`weather.db`) will be stored.

### Step 4: Run the Application

```bash
dotnet run
```

Open your browser and go to: **http://localhost:5034**

---

## 🐳 Running with Docker

### Build the Image

```bash
docker build -t weatherai .
```

### Run the Container

```bash
docker run -p 8080:8080 \
  -v weatherai-data:/app/data \
  -e WeatherSettings__GeminiKey=YOUR_GEMINI_KEY \
  -e WeatherSettings__OpenWeatherKey=YOUR_OPENWEATHER_KEY \
  weatherai
```

Open your browser and go to: **http://localhost:8080**

> The `-v weatherai-data:/app/data` flag ensures your search history and chat messages persist across container restarts.

---

## 🗂️ Project Structure

```
WeatherAI/
├── Components/
│   ├── Layout/
│   │   └── MainLayout.razor        # Split-pane layout (75/25)
│   ├── Pages/
│   │   └── Home.razor               # Main weather dashboard
│   └── Shared/
│       └── ChatWidget.razor          # AI Guardian chat panel
├── Models/
│   ├── WeatherRecord.cs              # Weather data model
│   ├── WeatherAnalysis.cs            # LINQ analysis results
│   ├── ChatMessage.cs                # Chat persistence model
│   └── SearchHistory.cs              # Search history model
├── Services/
│   ├── WeatherService.cs             # OpenWeatherMap + AI tips
│   ├── ChatService.cs                # Gemini AI chat (direct REST)
│   ├── WeatherAppState.cs            # Global state management
│   └── WeatherDbContext.cs           # EF Core SQLite context
├── wwwroot/
│   └── app.css                       # Glassmorphism + dynamic backgrounds
├── Program.cs                        # App configuration & DI
├── Dockerfile                        # Multi-stage Docker build
├── appsettings.json                  # API keys configuration
└── README.md
```

---

## 🔧 How It Works

1. **Search a City** — Type a city name or click a quick-select button (Islamabad, Lahore, Karachi, Murree)
2. **View Forecast** — See current weather, 5-day forecast grid, and AI-analyzed stats
3. **Chat with Guardian** — Ask the AI assistant about travel safety, packing tips, and driving conditions
4. **Dynamic Backgrounds** — The app background changes based on the weather condition (sunny, cloudy, rainy, etc.)
5. **Search History** — Click "History" to view your last 10 searches, click any to re-search

---

## 🛠️ Tech Stack

| Technology | Purpose |
|------------|---------|
| .NET 8 Blazor Server | Full-stack web framework |
| Entity Framework Core | ORM for SQLite database |
| SQLite | Local data persistence |
| Google Gemini 2.0 Flash | AI chat & lifestyle tips |
| OpenWeatherMap API | Real-time weather data |
| Bootstrap 5 | Responsive grid layout |
| Custom CSS | Glassmorphism design system |

---

## ⚠️ Troubleshooting

| Issue | Solution |
|-------|----------|
| "API Key Missing" in terminal | Check `appsettings.json` has valid keys |
| Weather search returns nothing | Verify your OpenWeatherMap key is active (can take a few hours after signup) |
| AI chat says "weather satellite" error | Check your Gemini key is valid at [aistudio.google.com](https://aistudio.google.com) |
| Database errors | Delete `data/weather.db` and restart — it will recreate automatically |
| Port 5034 in use | Change the port in `Properties/launchSettings.json` |

---

## 📄 License

MIT License — feel free to use, modify, and distribute.

---

**Made with ❤️ using .NET 8, Blazor, and Google Gemini AI**
