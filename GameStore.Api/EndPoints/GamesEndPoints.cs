using GameStore.Api.Mapping;

namespace GameStore.Api.EndPoints;

using Microsoft.AspNetCore.Builder;
using GameStore.Api.Dtos;
using GameStore.Api.Data;
using GameStore.Api.Entities;
    
public static class GamesEndPoints
{
    
    public static RouteGroupBuilder ConfigureEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        // why not use a database? because it's a simple example
        
        group.MapGet("/", (GameStoreContext dbContext) =>
        {
            var games = dbContext.Games
                .Select(game => new GameDto(
                    game.id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.Releasedate))
                .ToList();
            return games;
        });
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);
            return game is not null ? Results.Ok(game.toGameDetailsDto()) : Results.NotFound();
        }).WithName("GetGame");

        // POST
        group.MapPost("/", (CreateGameDots newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.toEntity();
            if (game.Genre is null)
            {
                return Results.BadRequest($"Genre with ID {newGame.GenreID} not found.");
            }
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            
            return Results.CreatedAtRoute("GetGame", new { id = game.id },game.toGameDetailsDto());
        });

        // put 
        // group.MapPut("/{id}", (int id, UpdateGameDto updateGameDto) =>
        // {
        //     var index = Games.FindIndex(game => game.id == id);
        //     Games[index] = new GameDto(
        //         id,
        //         updateGameDto.Name,
        //         updateGameDto.Genre,
        //         updateGameDto.Price,
        //         updateGameDto.ReleaseDate);
        //
        //     return Results.Accepted();
        // });
        //
        //
        // group.MapDelete("/{id}", (int id) =>
        // {
        //     Games.RemoveAll(game => game.id == id);
        //
        //     return Results.NoContent();
        // });

        return group;
    }
}