using Componentes.Data.Database.Models;

namespace Componentes.Data.Repositories.IRepositories;

public interface IUserRoleAssignmentRepository
{
    Task<UserRoleAssignment> CreateUserRoleAssignment(UserRoleAssignment roleAssignment);
    Task<UserRoleAssignment?> GetRoleAssignment(int roleId);
}