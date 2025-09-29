namespace GameStore.Api.Dtos;

public record GameSummaryDto(
    int id, 
    string Name, 
    string Genre, 
    decimal Price, 
    DateOnly ReleaseDate);
    