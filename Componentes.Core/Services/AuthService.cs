using AutoMapper;
using Componentes.Core.Services.IServices;
using Componentes.Data.Repositories.IRepositories;
using Componentes.Models.Configuration;
using Componentes.Models.DTO.UserDto;
using Componentes.Models.Requests.AuthRequest;
using Componentes.Utilities.Extensions;
using Microsoft.Extensions.Options;
using Componentes.Core.Logger;

namespace Componentes.Core.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly Jwt _jwtConfig;
        private readonly FileLogger _logger;

        public AuthService(IMapper mapper, IUserRepository userRepository, IOptions<ApplicationConfiguration> options,
            FileLogger logger) :
            base(mapper)
        {
            _userRepository = userRepository;
            _jwtConfig = options.Value.JwtConfig!;
            _logger = logger;
        }

        public async Task<string?> UserAuthentication(LoginRequest? loginRequest)
        {
            _logger.Log("UserAuthentication method called");

            if (loginRequest is null)
            {
                _logger.LogError("Login request is null");
                return null;
            }

            var user = await _userRepository.GetUserByEmail(loginRequest.Email);

            if (user != null && CommonExtensions.ValidatePass(user.PasswordHash, loginRequest.Password))
            {
                var userDto = Mapper.Map<UserDto>(user);

                var response = CommonExtensions.GenerateJwtToken(_jwtConfig, userDto);
                _logger.Log($"User authenticated successfully: {user.Email} + {response.ToJsonString()}");

                return response;
            }

            _logger.LogError($"Authentication failed for user: {loginRequest.ToJsonString()}");
            return null;
        }
    }
}