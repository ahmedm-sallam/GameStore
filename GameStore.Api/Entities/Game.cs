namespace GameStore.Api.Entities;

public class Game
{
    public int id { get; set; }
    public required string Name { get; set; }
    public int Genreid { get; set; }
    public Genre? Genre { get; set; }
    public decimal Price { get; set; }
    public DateOnly Releasedate { get; set; }
}