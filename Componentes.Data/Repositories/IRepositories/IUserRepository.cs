using Componentes.Data.Database.Models;

namespace Componentes.Data.Repositories.IRepositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user);
    Task<User?> GetUserByEmail(string email);
    Task<User> UpdateUserAsync(User user);
}