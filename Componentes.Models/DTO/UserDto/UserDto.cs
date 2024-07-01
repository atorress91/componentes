using Newtonsoft.Json;

namespace Componentes.Models.DTO.UserDto;

public class UserDto
{
    [JsonProperty("userId")] public int UserId { get; set; }

    [JsonProperty("email")] public string Email { get; set; } = null!;
    [JsonProperty("passwordHash")] public string PasswordHash { get; set; } = null!;

    [JsonProperty("name")] public string? Name { get; set; }

    [JsonProperty("lastName")] public string? LastName { get; set; }
    [JsonProperty("address")] public string? Address { get; set; }

    [JsonProperty("contactPreferences")] public string? ContactPreferences { get; set; }

    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; }

    [JsonProperty("deletedAt")] public DateTime? DeletedAt { get; set; }

    public virtual ICollection<UserRoleAssignmentDto.UserRoleAssignmentDto> UserRoleAssignments { get; set; } =
        new List<UserRoleAssignmentDto.UserRoleAssignmentDto>();
}