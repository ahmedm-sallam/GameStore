namespace GameStore.Api.Dtos;

public record GameDetailsDto(
    int id, 
    string Name, 
    int GenreId, 
    decimal Price, 
    DateOnly ReleaseDate);
    