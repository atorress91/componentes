using Componentes.Models.DTO.UserDto;
using Componentes.Models.Requests.UserRequest;

namespace Componentes.Core.Services.IServices;

public interface IUserService
{
    Task<UserDto?> CreateUser(UserRequest userRequest);
    Task<UserDto?> GetUserByEmail(string email);
}