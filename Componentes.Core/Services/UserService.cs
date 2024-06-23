using AutoMapper;
using Componentes.Core.Services.IServices;
using Componentes.Data.Database.Models;
using Componentes.Data.Repositories.IRepositories;
using Componentes.Models.DTO.UserDto;
using Componentes.Models.Requests.UserRequest;
using Componentes.Utilities.Extensions;

namespace Componentes.Core.Services;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IMapper mapper, IUserRepository userRepository) : base(mapper)
        => _userRepository = userRepository;


    public async Task<UserDto?> CreateUser(UserRequest userRequest)
    {
        if (userRequest == null)
            throw new ArgumentNullException(nameof(userRequest));

        if (string.IsNullOrWhiteSpace(userRequest.Email) || !CommonExtensions.IsValidEmail(userRequest.Email))
            throw new ArgumentException("Invalid email format", nameof(userRequest.Email));

        if (string.IsNullOrWhiteSpace(userRequest.Password))
            throw new ArgumentException("Password cannot be empty", nameof(userRequest.Password));

        var passwordEncrypt = CommonExtensions.EncryptPass(userRequest.Password);
        if (string.IsNullOrEmpty(passwordEncrypt))
            throw new InvalidOperationException("Failed to encrypt password");

        userRequest.Password = passwordEncrypt;

        var user = Mapper.Map<User>(userRequest);

        user = await _userRepository.CreateUser(user);

        return Mapper.Map<UserDto>(user);
    }
    
    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);

        return Mapper.Map<UserDto>(user);
    }
}