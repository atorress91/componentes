using AutoMapper;
using Componentes.Core.Logger;
using Componentes.Core.Services.IServices;
using Componentes.Data.Database.Models;
using Componentes.Data.Repositories.IRepositories;
using Componentes.Models.DTO.UserDto;
using Componentes.Models.Requests.UserRequest;
using Componentes.Utilities.Extensions;

namespace Componentes.Core.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleAssignmentRepository _roleAssignmentRepository;
        private readonly FileLogger _logger;

        public UserService(IMapper mapper, IUserRepository userRepository,
            IUserRoleAssignmentRepository roleAssignmentRepository, FileLogger logger) : base(mapper)
        {
            _userRepository = userRepository;
            _roleAssignmentRepository = roleAssignmentRepository;
            _logger = logger;
        }

        public async Task<UserDto?> CreateUser(UserRequest userRequest)
        {
            _logger.Log($"CreateUser method called: {userRequest.ToJsonString()}");

            if (userRequest == null)
            {
                _logger.LogError("User request is null");
                throw new ArgumentNullException(nameof(userRequest));
            }

            if (string.IsNullOrWhiteSpace(userRequest.Email) || !CommonExtensions.IsValidEmail(userRequest.Email))
            {
                _logger.LogError("Invalid email format");
                throw new ArgumentException("Invalid email format", nameof(userRequest.Email));
            }

            if (string.IsNullOrWhiteSpace(userRequest.Password))
            {
                _logger.LogError("Password cannot be empty");
                throw new ArgumentException("Password cannot be empty", nameof(userRequest.Password));
            }

            var passwordEncrypt = CommonExtensions.EncryptPass(userRequest.Password);
            if (string.IsNullOrEmpty(passwordEncrypt))
            {
                _logger.LogError("Failed to encrypt password");
                throw new InvalidOperationException("Failed to encrypt password");
            }

            userRequest.Password = passwordEncrypt;

            var user = Mapper.Map<User>(userRequest);

            user = await _userRepository.CreateUser(user);
            _logger.Log($"User created with ID: {user.UserId}");

            await _roleAssignmentRepository.CreateUserRoleAssignment(new UserRoleAssignment
            {
                UserId = user.UserId,
                RoleId = userRequest.RolId
            });
            _logger.Log($"Role assignment created for user ID: {user.UserId}");

            return Mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByEmail(string email)
        {
            _logger.Log($"GetUserByEmail method called with email: {email}");
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
                _logger.LogError($"User not found with email: {email}");

            return Mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>?> GetAllUsers()
        {
            _logger.Log("GetAllUsers method called");
            var users = await _userRepository.GetAllUsers();

            if (users == null || users.Count == 0)
                _logger.LogError("No users found");


            return Mapper.Map<List<UserDto>>(users);
        }
    }
}