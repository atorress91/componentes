using Componentes.Data.Database.Models;

namespace Componentes.Data.Repositories.IRepositories;

public interface IProductRepository
{
    Task<Product?> CreateProductAsync(Product product);
    Task<Product?> GetProductById(int id);
    Task<ICollection<Product>?> GetProductsByCategoryId(string categoryName);
    Task<Product?> UpdateProductAsync(Product product);
    Task<Product?> DeleteProductAsync(Product product);
    Task<List<Product>> GetAllProducts();
}