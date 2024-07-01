using Componentes.Data.Database;
using Componentes.Data.Database.Models;
using Componentes.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Componentes.Data.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(ProyectoComponentesContext context) : base(context)
    {
    }

    public async Task<User> CreateUser(User user)
    {
        var today = DateTime.Now;
        user.CreatedAt = today;
        user.UpdatedAt = today;

        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        return user;
    }

    public async Task<ICollection<User>?> GetAllUsers()
        => await Context.Users.ToListAsync();

    public Task<User?> GetUserByEmail(string email)
        => Context.Users.Include(x => x.UserRoleAssignments).FirstOrDefaultAsync(x => x.Email == email);

    public async Task<User> UpdateUserAsync(User user)
    {
        user.UpdatedAt = DateTime.Now;

        Context.Users.Update(user);
        await Context.SaveChangesAsync();

        return user;
    }
}