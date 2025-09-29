using GameStore.Api.EndPoints;
using GameStore.Api.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load();

var connectionString = $"Host={Env.GetString("DB_HOST")};" +
                       $"Port={Env.GetString("DB_PORT")};" +
                       $"Database={Env.GetString("DB_NAME")};" +
                       $"Username={Env.GetString("DB_USER")};" +
                       $"Password={Env.GetString("DB_PASSWORD")};";

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseNpgsql(connectionString));

// Add controllers (if you're using MVC/API controllers)
builder.Services.AddControllers();

// Add API explorers
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "GameStore API", 
        Version = "v1",
        Description = "API for managing game store inventory"
    });
});

var app = builder.Build();

// Configure endpoints
app.ConfigureEndPoints();

// Enable Swagger in all environments for now (for testing)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GameStore API v1");
    options.RoutePrefix = "swagger"; // Makes swagger available at /swagger
});

app.MigrateDb();
app.Run();