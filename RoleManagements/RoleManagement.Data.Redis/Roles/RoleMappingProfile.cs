using AutoMapper;
using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Types;

namespace RoleManagement.Data.Redis.Roles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<string, RoleId>().ConvertUsing(s => RoleId.From(s));
        CreateMap<RoleId, string>().ConvertUsing(s => s.Value);

        CreateMap<Role, RoleDataModel>()
            .ReverseMap()
            .ForMember("_meta", expression => expression.MapFrom(model => model.Meta))
            .ForMember(group => group.Meta, expression => expression.Ignore())
            .ForMember("_permissions", expression => expression.MapFrom(model => model.Permissions))
            .ForMember(group => group.Permissions, expression => expression.Ignore());
        
    }
}