using Microsoft.EntityFrameworkCore;
using WeatherAI.Models;

namespace WeatherAI.Services;

public class WeatherDbContext : DbContext
{
    public DbSet<SearchHistory> SearchHistory { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }
}
