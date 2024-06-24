using AutoMapper;
using Componentes.Data.Database.Models;
using Componentes.Models.DTO.UserDto;
using Componentes.Models.DTO.UserRoleAssignmentDto;
using Componentes.Models.Requests.UserRequest;

namespace Componentes.Core.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        MapDto();
    }

    private void MapDto()
    {
        CreateMap<UserRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.ShoppingCarts, opt => opt.Ignore())
            .ForMember(dest => dest.UserRoleAssignments, opt => opt.Ignore());

        CreateMap<User, UserDto>();
        CreateMap<UserRoleAssignment, UserRoleAssignmentDto>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        CreateMap<UserRoleAssignmentDto, UserRoleAssignment>()
            .ForMember(s => s.Role, map => map.Ignore())
            .ForMember(s => s.User, map => map.Ignore());
    }
}