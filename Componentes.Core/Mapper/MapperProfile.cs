using AutoMapper;
using Componentes.Data.Database.Models;
using Componentes.Models.DTO.ProductDto;
using Componentes.Models.DTO.UserDto;
using Componentes.Models.DTO.UserRoleAssignmentDto;
using Componentes.Models.Requests.ProductRequest;
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
        CreateMap<User, UserDto>();
        CreateMap<ProductRequest, Product>();
        CreateMap<Product, ProductDto>();

        CreateMap<UserRequest, User>()
            .ForMember(x => x.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
            .ForMember(x => x.DeletedAt, opt => opt.Ignore())
            .ForMember(x => x.Orders, opt => opt.Ignore())
            .ForMember(x => x.ShoppingCarts, opt => opt.Ignore())
            .ForMember(x => x.UserRoleAssignments, opt => opt.Ignore());


        CreateMap<UserRoleAssignment, UserRoleAssignmentDto>()
            .ForMember(x => x.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId));

        CreateMap<UserRoleAssignmentDto, UserRoleAssignment>()
            .ForMember(x => x.Role, map => map.Ignore())
            .ForMember(x => x.User, map => map.Ignore());
    }
}