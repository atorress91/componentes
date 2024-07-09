using AutoMapper;
using Componentes.Core.Services.IServices;
using Componentes.Data.Database.Models;
using Componentes.Data.Repositories.IRepositories;
using Componentes.Models.DTO.ProductDto;
using Componentes.Models.Requests.ProductRequest;

namespace Componentes.Core.Services;

public class ProductService : BaseService, IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IMapper mapper, IProductRepository productRepository) : base(mapper)
        => _productRepository = productRepository;


    public async Task<ProductDto?> CreateProductAsync(ProductRequest request)
    {
        var product = Mapper.Map<Product>(request);

        var result = await _productRepository.CreateProductAsync(product);

        if (result is null)
            return null;

        return Mapper.Map<ProductDto>(result);
    }

    public async Task<ProductDto?> UpdateProductAsync(ProductRequest request)
    {
        var product = await _productRepository.GetProductById(request.Id);

        if (product is null)
            return null;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.ImagePath = request.ImagePath;
        product.Category = request.Category;

        var updateResult = await _productRepository.UpdateProductAsync(product);

        if (updateResult is null)
            return null;

        return Mapper.Map<ProductDto>(updateResult);
    }

    public async Task<List<ProductDto>> GetAllProducts()
    {
        var response = await _productRepository.GetAllProducts();

        return Mapper.Map<List<ProductDto>>(response);
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var product = await _productRepository.GetProductById(productId);

        if (product is null)
            return false;

        var result = await _productRepository.DeleteProductAsync(product);

        if (result is null)
            return false;
        
        return true;
    }
}