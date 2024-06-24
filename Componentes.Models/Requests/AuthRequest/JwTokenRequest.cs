using Componentes.Models.DTO.UserDto;

namespace Componentes.Models.Requests.AuthRequest;

public class JwTokenRequest
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;

    public UserDto User { get; set; } = null!;
}