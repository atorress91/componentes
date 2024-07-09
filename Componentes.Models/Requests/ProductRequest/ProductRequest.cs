namespace Componentes.Models.Requests.ProductRequest;

public class ProductRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public string? ImagePath { get; set; }
}