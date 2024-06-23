using AutoMapper;

namespace Componentes.Core.Services;

public class BaseService
{
    protected readonly IMapper Mapper;

    protected BaseService(IMapper mapper)
        => Mapper = mapper;
}