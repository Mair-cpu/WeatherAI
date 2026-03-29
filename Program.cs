using WeatherAI.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<WeatherAI.Services.WeatherService>();
builder.Services.AddScoped<WeatherAI.Services.WeatherAppState>();
builder.Services.AddHttpClient<WeatherAI.Services.ChatService>();

builder.Services.AddDbContext<WeatherAI.Services.WeatherDbContext>(options =>
    options.UseSqlite("Data Source=data/weather.db"));

var app = builder.Build();

// Create DB if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WeatherAI.Services.WeatherDbContext>();
    db.Database.EnsureCreated();
    
    db.ChatMessages.RemoveRange(db.ChatMessages);
    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
