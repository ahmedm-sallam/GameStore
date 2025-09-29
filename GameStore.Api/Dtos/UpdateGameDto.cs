using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class UpdateGameDto(
    [Required]  string Name,
    int GenreID,
    [Required] decimal Price,
    [Required] DateOnly ReleaseDate
);