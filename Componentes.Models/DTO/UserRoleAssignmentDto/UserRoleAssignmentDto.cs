namespace Componentes.Models.DTO.UserRoleAssignmentDto;

public class UserRoleAssignmentDto
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}