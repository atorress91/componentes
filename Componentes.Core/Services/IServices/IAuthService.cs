using Componentes.Models.Requests.AuthRequest;

namespace Componentes.Core.Services.IServices;

public interface IAuthService
{
    Task<string?> UserAuthentication(LoginRequest? loginRequest);
}