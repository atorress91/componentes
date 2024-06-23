using AutoMapper;
using Componentes.Core.Services.IServices;
using Componentes.Data.Repositories.IRepositories;

namespace Componentes.Core.Services;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IMapper mapper, IUserRepository userRepository) : base(mapper)
        => _userRepository = userRepository;
}