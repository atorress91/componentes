namespace Componentes.Data.Repositories.IRepositories;

using Componentes.Data.Models;

public interface IUserRepository
{
    Task<User> CreateUser(User user);
    Task<User?> GetUserByEmail(string email);
    Task<User> UpdateUserAsync(User user);
}