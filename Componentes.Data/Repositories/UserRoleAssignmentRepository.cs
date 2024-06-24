using Componentes.Data.Database;
using Componentes.Data.Database.Models;
using Componentes.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Componentes.Data.Repositories;

public class UserRoleAssignmentRepository : BaseRepository, IUserRoleAssignmentRepository
{
    public UserRoleAssignmentRepository(ProyectoComponentesContext context) : base(context)
    {
    }

    public async Task<UserRoleAssignment> CreateUserRoleAssignment(UserRoleAssignment roleAssignment)
    {
        var today = DateTime.Now;
        roleAssignment.CreatedAt = today;
        roleAssignment.UpdatedAt = today;

        await Context.UserRoleAssignments.AddAsync(roleAssignment);
        await Context.SaveChangesAsync();

        return roleAssignment;
    }

    public async Task<UserRoleAssignment?> GetRoleAssignment(int roleId)
        => await Context.UserRoleAssignments.FirstOrDefaultAsync(x => x.RoleId == roleId);
}