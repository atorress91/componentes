using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Componentes.Api.Controllers;
using Componentes.Models.Responses;


namespace Componentes.Test
{
    public class LogsControllerTests
    {
        private readonly LogsController _controller;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment = new Mock<IWebHostEnvironment>();
        private readonly string _tempDirectory;

        public LogsControllerTests()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDirectory);

            _mockEnvironment.Setup(e => e.ContentRootPath).Returns(_tempDirectory);
            _controller = new LogsController(_mockEnvironment.Object);
        }

        [Fact]
        public void GetAllLogs_ReturnsOk_WithLogs_WhenLogFileExists()
        {
            string logFilePath = Path.Combine(_tempDirectory, "app_log.txt");
            File.WriteAllLines(logFilePath, new string[] { "Log1", "Log2" });
            
            var result = _controller.GetAllLogs();
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serviceResponse = Assert.IsType<ServicesResponse>(okResult.Value);
            Assert.NotNull(serviceResponse.Data);  
            
            var logs = serviceResponse.Data as string[];
            Assert.NotNull(logs);  
            Assert.Contains("Log1", logs);  
        }

        [Fact]
        public void GetAllLogs_ReturnsNotFound_WhenLogFileDoesNotExist()
        {
            var result = _controller.GetAllLogs();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }
    }
}