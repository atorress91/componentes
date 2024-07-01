using Componentes.Core.Services.IServices;
using Componentes.Models.Requests.UserRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Componentes.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
        => _userService = userService;


    [HttpGet("get_user_by_email/{email}")]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
    {
        var result = await _userService.GetUserByEmail(email);

        return result is null ? BadRequest(Fail("The user wasn't found.")) : Ok(Success(result));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest)
    {
        var result = await _userService.CreateUser(userRequest);

        return result is null ? Ok(Fail("The user wasn't created")) : Ok(Success(result));
    }

    [HttpGet("get_all_users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsers();

        return result is null ? Ok(Fail("No users found")) : Ok(Success(result));
    }
}