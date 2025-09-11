namespace GameStore.Api.EndPoints;

using Microsoft.AspNetCore.Builder;
using GameStore.Api.Dtos;
using GameStore.Api.Data;
using GameStore.Api.Entities;
    
public static class GamesEndPoints
{
    private static readonly List<GameDto> Games = new List<GameDto>
    {
        new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
        new GameDto(2, "Super Mario Odyssey", "Platform", 49.99m, new DateOnly(2017, 10, 27)),
        new GameDto(3, "God of War", "Action-adventure", 39.99m, new DateOnly(2018, 4, 20)),
        new GameDto(4, "Red Dead Redemption 2", "Action-adventure", 59.99m, new DateOnly(2018, 10, 26)),
        new GameDto(5, "The Witcher 3: Wild Hunt", "Action RPG", 29.99m, new DateOnly(2015, 5, 19))
    };


    public static RouteGroupBuilder ConfigureEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        // why not use a database? because it's a simple example
        
        group.MapGet("/", () => Games);
        group.MapGet("/{id}", (int id) =>
        {
            var game = Games.Find(game => game.id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName("GetGame");

        // POST
        group.MapPost("/", (CreateGameDots newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreID) ?? throw new Exception("Genre not found"),
                Genreid = newGame.GenreID,
                Price = newGame.Price,
                Releasedate = newGame.ReleaseDate
            };
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            
            GameDto gameDto = new(game.id, game.Name, game.Genre!.Name, game.Price, game.Releasedate);
            
            return Results.CreatedAtRoute("GetGame", new { id = game.id }, gameDto);
        });

        // put 
        group.MapPut("/{id}", (int id, UpdateGameDto updateGameDto) =>
        {
            var index = Games.FindIndex(game => game.id == id);
            Games[index] = new GameDto(
                id,
                updateGameDto.Name,
                updateGameDto.Genre,
                updateGameDto.Price,
                updateGameDto.ReleaseDate);

            return Results.Accepted();
        });


        group.MapDelete("/{id}", (int id) =>
        {
            Games.RemoveAll(game => game.id == id);

            return Results.NoContent();
        });

        return group;
    }
}