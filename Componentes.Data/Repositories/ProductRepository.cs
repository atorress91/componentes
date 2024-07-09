using Componentes.Data.Database;
using Componentes.Data.Database.Models;
using Componentes.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Componentes.Data.Repositories;

public class ProductRepository : BaseRepository, IProductRepository
{
    public ProductRepository(ProyectoComponentesContext context) : base(context)
    {
    }

    public async Task<Product?> CreateProductAsync(Product product)
    {
        var today = DateTime.Now;

        product.CreatedAt = today;
        product.UpdatedAt = today;

        await Context.Products.AddAsync(product);
        await Context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        var today = DateTime.Now;

        product.UpdatedAt = today;

        Context.Products.Update(product);
        await Context.SaveChangesAsync();

        return product;
    }

    public Task<List<Product>> GetAllProducts()
        => Context.Products.AsNoTracking().ToListAsync();

    public Task<Product?> GetProductById(int id)
        => Context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

    public async Task<ICollection<Product>?> GetProductsByCategoryId(string categoryName)
        => await Context.Products.Where(x => x.Category == categoryName).ToListAsync();

    public async Task<Product?> DeleteProductAsync(Product product)
    {
        var today = DateTime.Now;

        product.UpdatedAt = today;
        product.DeletedAt = today;

        Context.Products.Update(product);
        await Context.SaveChangesAsync();

        return product;
    }
}