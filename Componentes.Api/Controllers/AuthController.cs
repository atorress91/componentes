using Componentes.Core.Services.IServices;
using Componentes.Models.Requests.AuthRequest;
using Microsoft.AspNetCore.Mvc;

namespace Componentes.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
        => _authService = authService;

    [HttpPost]
    public async Task<IActionResult> UserAuthentication([FromBody] LoginRequest loginRequest)
    {
        var result = await _authService.UserAuthentication(loginRequest);

        return string.IsNullOrEmpty(result) ? BadRequest(Fail("The user is not authenticated.")) : Ok(Success(result));
    }
}