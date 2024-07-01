using AutoMapper;
using Componentes.Core.Services.IServices;
using Componentes.Data.Repositories.IRepositories;
using Componentes.Models.Configuration;
using Componentes.Models.DTO.UserDto;
using Componentes.Models.Requests.AuthRequest;
using Componentes.Utilities.Extensions;
using Microsoft.Extensions.Options;

namespace Componentes.Core.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly Jwt _jwtConfig;

    public AuthService(IMapper mapper, IUserRepository userRepository, IOptions<ApplicationConfiguration> options) :
        base(mapper)
    {
        _userRepository = userRepository;
        _jwtConfig = options.Value.JwtConfig!;
    }

    public async Task<string?> UserAuthentication(LoginRequest? loginRequest)
    {
        if (loginRequest is null)
            return null;

        var user = await _userRepository.GetUserByEmail(loginRequest.Email);

        if (user != null && CommonExtensions.ValidatePass(user.PasswordHash, loginRequest.Password))
        {
            var userDto = Mapper.Map<UserDto>(user);

            return CommonExtensions.GenerateJwtToken(_jwtConfig, userDto);
        }

        return null;
    }
}