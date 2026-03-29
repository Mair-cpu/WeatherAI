# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies first (layer caching)
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create persistent data directory for SQLite
RUN mkdir -p /app/data
VOLUME /app/data

# Copy published output
COPY --from=build /app/publish .

# Expose port 8080
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV GEMINI_KEY=""
ENV OPENWEATHER_KEY=""

ENTRYPOINT ["dotnet", "WeatherAI.dll"]
