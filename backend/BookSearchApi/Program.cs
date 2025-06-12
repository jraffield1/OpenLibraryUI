using System.Text.Json;
using BookSearchLib;

var builder = WebApplication.CreateBuilder(args);

// Logging Configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Service Registration
builder.Services.AddControllers()
                .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register BookSearcher as a typed HttpClient —
builder.Services
    .AddHttpClient<BookSearcher>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5));


builder.Services.AddHealthChecks();

// CORS for front-end service
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Set up middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
