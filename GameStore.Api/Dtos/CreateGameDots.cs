using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDots(
    [Required] string Name,
    int GenreID,
    [Required] decimal Price,
    [Required] DateOnly ReleaseDate
);
