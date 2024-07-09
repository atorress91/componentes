using Componentes.Core.Services.IServices;
using Componentes.Models.Requests.ProductRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Componentes.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class ProductController : BaseController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
        => _productService = productService;

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] ProductRequest request)
    {
        var result = await _productService.CreateProductAsync(request);

        return result is null ? Ok(Fail("The product wasn't created")) : Ok(Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] ProductRequest request)
    {
        var result = await _productService.UpdateProductAsync(request);

        return result is null ? Ok(Fail("The product wasn't updated")) : Ok(Success(result));
    }

    [HttpGet("get_all_products")]
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await _productService.GetAllProducts();

        return Ok(Success(result));
    }

    [HttpPost("delete_product")]
    public async Task<IActionResult> DeleteProduct([FromBody] int productId)
    {
        var result = await _productService.DeleteProductAsync(productId);

        return Ok(Success(result));
    }
}