using Componentes.Api.Controllers;
using Componentes.Core.Services.IServices;
using Componentes.Models.DTO.ProductDto;
using Componentes.Models.Requests.ProductRequest;
using Componentes.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Componentes.Test;

public class ProductControllerTest
{
    private readonly ProductController _controller;
    private readonly Mock<IProductService> _mockProductService = new Mock<IProductService>();

    public ProductControllerTest()
    {
        _controller = new ProductController(_mockProductService.Object);
    }

    [Fact]
    public async Task CreateUser_ReturnsOk_WhenProductIsCreatedSuccessfully()
    {
        var request = new ProductRequest { Name = "New Product", Price = 20.00m };
        var response = new ProductDto { ProductId = 1, Name = "New Product", Price = 20.00m };
        _mockProductService.Setup(x => x.CreateProductAsync(request)).ReturnsAsync(response);

        var result = await _controller.CreateUser(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var serviceResponse = Assert.IsType<ServicesResponse>(okResult.Value);
        Assert.True(serviceResponse.Success);
        Assert.Equal(response, serviceResponse.Data);
    }

    [Fact]
    public async Task CreateUser_ReturnsOkWithFailureMessage_WhenProductCreationFails()
    {
        var request = new ProductRequest { Name = "New Product", Price = 20.00m };
        _mockProductService.Setup(x => x.CreateProductAsync(request)).ReturnsAsync((ProductDto)null);

        var result = await _controller.CreateUser(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var serviceResponse = Assert.IsType<ServicesResponse>(okResult.Value);
        Assert.False(serviceResponse.Success);
        Assert.Equal("The product wasn't created", serviceResponse.Message);
    }

    [Fact]
    public async Task UpdateUser_ReturnsOk_WhenProductIsUpdatedSuccessfully()
    {
        var request = new ProductRequest { Id = 1, Name = "Updated Product", Price = 30.00m };
        var response = new ProductDto { ProductId = 1, Name = "Updated Product", Price = 30.00m };
        _mockProductService.Setup(x => x.UpdateProductAsync(request)).ReturnsAsync(response);

        var result = await _controller.UpdateUser(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var serviceResponse = Assert.IsType<ServicesResponse>(okResult.Value);
        Assert.True(serviceResponse.Success);
        Assert.Equal(response, serviceResponse.Data);
    }

    [Fact]
    public async Task UpdateUser_ReturnsOkWithFailureMessage_WhenProductUpdateFails()
    {
        var request = new ProductRequest { Id = 1, Name = "Updated Product", Price = 30.00m };
        _mockProductService.Setup(x => x.UpdateProductAsync(request)).ReturnsAsync((ProductDto)null);

        var result = await _controller.UpdateUser(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var serviceResponse = Assert.IsType<ServicesResponse>(okResult.Value);
        Assert.False(serviceResponse.Success);
        Assert.Equal("The product wasn't updated", serviceResponse.Message);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsOk_WhenProductsAreAvailable()
    {
        var products = new List<ProductDto>
        {
            new ProductDto { ProductId = 1, Name = "Product 1", Price = 100.00m },
            new ProductDto { ProductId = 2, Name = "Product 2", Price = 150.00m }
        };
        _mockProductService.Setup(x => x.GetAllProducts()).ReturnsAsync(products);

        var result = await _controller.GetAllProducts();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var serviceResponse = Assert.IsType<ServicesResponse>(okResult.Value);
        Assert.True(serviceResponse.Success);
        Assert.Equal(products, serviceResponse.Data);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsOk_WhenProductIsDeletedSuccessfully()
    {
        int productId = 1;
        _mockProductService.Setup(service => service.DeleteProductAsync(productId))
            .ReturnsAsync(false); 

        var result = await _controller.DeleteProduct(productId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var serviceResponse = Assert.IsType<ServicesResponse>(okResult.Value);
        Assert.True(serviceResponse.Success);
        Assert.False((bool)serviceResponse.Data);
    }
}