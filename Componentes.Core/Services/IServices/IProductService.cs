using Componentes.Models.DTO.ProductDto;
using Componentes.Models.Requests.ProductRequest;

namespace Componentes.Core.Services.IServices;

public interface IProductService
{
    Task<ProductDto?> CreateProductAsync(ProductRequest request);
    Task<ProductDto?> UpdateProductAsync(ProductRequest request);
    Task<List<ProductDto>> GetAllProducts();
    Task<bool> DeleteProductAsync(int productId);
}