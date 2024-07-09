
using Componentes.Api.Controllers;
using Componentes.Core.Services.IServices;
using Componentes.Models.Requests.AuthRequest;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Componentes.Test
{
    public class AuthControllerTest
    {
        private readonly AuthController _controller;
        private readonly Mock<IAuthService> _mockAuthService = new Mock<IAuthService>();

        public AuthControllerTest()
        {
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task UserAuthentication_ReturnsBadRequest_WhenUserIsNotAuthenticated()
        {
            var loginRequest = new LoginRequest { Email = "test", Password = "password" };
            _mockAuthService.Setup(s => s.UserAuthentication(It.IsAny<LoginRequest>())).ReturnsAsync(string.Empty);
            
            var result = await _controller.UserAuthentication(loginRequest);
            
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UserAuthentication_ReturnsOk_WhenUserIsAuthenticated()
        {
            var loginRequest = new LoginRequest { Email = "admin@gmail.com", Password = "Test123$" };
            _mockAuthService.Setup(s => s.UserAuthentication(It.IsAny<LoginRequest>())).ReturnsAsync("token123");
            
            var result = await _controller.UserAuthentication(loginRequest);
            
            Assert.IsType<OkObjectResult>(result);
        }
    }
}