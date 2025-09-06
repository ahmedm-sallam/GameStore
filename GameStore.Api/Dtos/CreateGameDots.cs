using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDots(
    [Required] string Name,
    [Required] string Genre,
    [Required] decimal Price,
    [Required] DateOnly ReleaseDate
);
