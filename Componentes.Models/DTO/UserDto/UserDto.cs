namespace Componentes.Models.DTO.UserDto;

public class UserDto
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? ContactPreferences { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<UserRoleAssignmentDto.UserRoleAssignmentDto> UserRoleAssignments { get; set; } =
        new List<UserRoleAssignmentDto.UserRoleAssignmentDto>();
}