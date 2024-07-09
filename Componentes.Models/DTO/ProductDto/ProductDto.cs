using Newtonsoft.Json;

namespace Componentes.Models.DTO.ProductDto;

public class ProductDto
{
    [JsonProperty("productId")] public int ProductId { get; set; }

    [JsonProperty("name")] public string Name { get; set; } = null!;

    [JsonProperty("description")] public string? Description { get; set; }

    [JsonProperty("price")] public decimal Price { get; set; }

    [JsonProperty("category")] public string? Category { get; set; }

    [JsonProperty("imagePath")] public string? ImagePath { get; set; }

    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; }

    [JsonProperty("deletedAt")] public DateTime? DeletedAt { get; set; }
}