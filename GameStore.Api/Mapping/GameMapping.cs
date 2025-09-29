namespace GameStore.Api.Mapping;

using GameStore.Api.Entities;
using GameStore.Api.Dtos;


public static class GameMapping
{
    public static Game toEntity(this CreateGameDots game)
    {
        return new Game()
        {
            Name = game.Name,
            Genreid = game.GenreID,
            Price = game.Price,
            Releasedate = game.ReleaseDate
        };
    }
    
    public static Game toEntity(this UpdateGameDto game, int ID)
    {
        return new Game()
        {
            id = ID,
            Name = game.Name,
            Genreid = game.GenreID,
            Price = game.Price,
            Releasedate = game.ReleaseDate
        };
    }
    public static GameSummaryDto toGameSummaryDto(this Game game)
    {
        return new (
            game.id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.Releasedate
        );
    }
    
    public static GameDetailsDto toGameDetailsDto(this Game game)
    {
        return new (
            game.id,
            game.Name,
            game.Genreid,
            game.Price,
            game.Releasedate
        );
    }
}