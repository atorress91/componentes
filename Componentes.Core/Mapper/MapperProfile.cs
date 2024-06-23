using AutoMapper;
using Componentes.Data.Database.Models;
using Componentes.Models.DTO.UserDto;

namespace Componentes.Core.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        MapDto();
    }

    private void MapDto()
    {
        CreateMap<User, UserDto>();
    }
}