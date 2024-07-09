using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Componentes.Api.Controllers;

[Controller]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class LogsController : BaseController
{
    private readonly string _logFilePath;
    private readonly IWebHostEnvironment _environment;

    public LogsController(IWebHostEnvironment environment)
    {
        _environment = environment;
        _logFilePath = Path.Combine(_environment.ContentRootPath, "app_log.txt");
    }

    [HttpGet("get_all_logs")]
    public IActionResult GetAllLogs()
    {
        if (!System.IO.File.Exists(_logFilePath))
        {
            return NotFound($"Log file not found at: {_logFilePath}");
        }

        var logs = System.IO.File.ReadAllLines(_logFilePath);
        return Ok(Success(logs));
    }

    [HttpPost("clear_logs")]
    public IActionResult ClearLogs()
    {
        try
        {
            if (!System.IO.File.Exists(_logFilePath))
            {
                return NotFound($"Log file not found at: {_logFilePath}");
            }

            System.IO.File.WriteAllText(_logFilePath, string.Empty);

            return Ok(Success("Logs cleared successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while clearing the logs" + ex);
        }
    }
}