using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

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
            dbContext.Games
                .Include(game => game.Genre )
                .OrderBy(game => game.id)
                .Select(game => game.toGameSummaryDto())
                .AsNoTracking()
                .ToListAsync()
            
        );
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(game.toGameDetailsDto()) : Results.NotFound();
        }).WithName("GetGame");

        // POST
        group.MapPost("/", async (CreateGameDots newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.toEntity();
            if (game.Genre is null)
            {
                return Results.BadRequest($"Genre with ID {newGame.GenreID} not found.");
            }
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            
            return Results.CreatedAtRoute("GetGame", new { id = game.id },game.toGameDetailsDto());
        });

        // put 
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGameDto, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            
            dbContext.Entry(game).CurrentValues.SetValues(updateGameDto.toEntity(id));
            await dbContext.SaveChangesAsync();
            
            return Results.Accepted();
        });
        
        
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                .Where(game => game.id == id)
                .ExecuteDeleteAsync();
            return Results.Accepted("Game deleted");
        });

        return group;
    }
}