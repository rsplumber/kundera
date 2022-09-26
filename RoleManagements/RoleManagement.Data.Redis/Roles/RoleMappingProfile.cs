using AutoMapper;
using RoleManagements.Domain.Roles;

namespace RoleManagement.Data.Redis.Roles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Role, RoleDataModel>()
            .ForMember(dest => dest.Meta, opt => opt.MapFrom("_meta"));
    }
}