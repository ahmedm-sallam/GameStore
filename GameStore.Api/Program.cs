using GameStore.Api.EndPoints;
using GameStore.Api.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
// postgresql 
Env.Load();

var connectionString = $"Host={Env.GetString("DB_HOST")};" +
                       $"Port={Env.GetString("DB_PORT")};" +
                       $"Database={Env.GetString("DB_NAME")};" +
                       $"Username={Env.GetString("DB_USER")};" +
                       $"Password={Env.GetString("DB_PASSWORD")};";


builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();
app.ConfigureEndPoints();

app.MigrateDb();
app.Run();