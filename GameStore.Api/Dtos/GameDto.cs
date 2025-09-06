namespace GameStore.Api.Dtos;

public record GameDto(
    int id, 
    string Name, 
    string Genre, 
    decimal Price, 
    DateOnly ReleaseDate);
    