using GameStore.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.ConfigureEndPoints();
app.Run();