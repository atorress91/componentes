namespace Componentes.Models.Requests.UserRequest;

public class UserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? ContactPreferences { get; set; }
    public int RolId { get; set; }
}