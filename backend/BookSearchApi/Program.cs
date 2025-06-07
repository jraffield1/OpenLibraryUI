using BookSearchLib;

var builder = WebApplication.CreateBuilder(args);

// Register all MVC Controllers
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new BookSearcher(new HttpClient()));
builder.Services.AddHealthChecks();

// CORS integration for interaction with frontend
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();