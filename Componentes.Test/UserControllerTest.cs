using Moq;
using Componentes.Core.Services.IServices;
using Componentes.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Componentes.Models.DTO.UserDto;
using Componentes.Models.Requests.UserRequest;
using Componentes.Models.Responses;

namespace Componentes.Test
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTest()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task CreateUser_ReturnsOk_WhenUserIsCreatedSuccessfully()
        {
            var userRequest = new UserRequest { Email = "test@example.com", Password = "ValidPassword123!", RolId = 1 };
            var userDto = new UserDto { Email = userRequest.Email };
            _mockUserService.Setup(x => x.CreateUser(userRequest)).ReturnsAsync(userDto);

            var result = await _controller.CreateUser(userRequest);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.IsType<ServicesResponse>(okResult.Value);
            var serviceResponse = (ServicesResponse)okResult.Value;
            Assert.True(serviceResponse.Success);
            Assert.Equal(userDto, serviceResponse.Data);
        }

        [Fact]
        public async Task CreateUser_ReturnsOkWithFailureMessage_WhenUserIsNotCreated()
        {
            var userRequest = new UserRequest { Email = "invalid@example.com", Password = "password" };
            _mockUserService.Setup(x => x.CreateUser(userRequest)).ReturnsAsync((UserDto)null);  
            
            var result = await _controller.CreateUser(userRequest);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serviceResponse = Assert.IsType<ServicesResponse>(okResult.Value);
            Assert.False(serviceResponse.Success);
            Assert.Equal("The user wasn't created", serviceResponse.Message);
        }


        [Fact]
        public async Task GetUserByEmail_ReturnsBadRequest_WhenUserNotFound()
        {
            string email = "test@example.com";
            _mockUserService.Setup(service => service.GetUserByEmail(email)).ReturnsAsync((UserDto)null);

            var result = await _controller.GetUserByEmail(email);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<ServicesResponse>(badRequestResult.Value);
            var serviceResponse = (ServicesResponse)badRequestResult.Value;
            Assert.False(serviceResponse.Success);
            Assert.Equal("The user wasn't found.", serviceResponse.Message);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnsOk_WhenUserFound()
        {
            string email = "test@example.com";
            var userDto = new UserDto { Email = email };
            _mockUserService.Setup(service => service.GetUserByEmail(email)).ReturnsAsync(userDto);

            var result = await _controller.GetUserByEmail(email);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var serviceResponse = (ServicesResponse)okResult.Value;
            Assert.True(serviceResponse.Success);
            Assert.Equal(userDto, serviceResponse.Data);
        }
    }
}