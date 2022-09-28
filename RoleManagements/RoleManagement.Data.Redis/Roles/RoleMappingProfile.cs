using AutoMapper;
using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Types;

namespace RoleManagement.Data.Redis.Roles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<string, RoleId>().ConvertUsing(s => RoleId.From(s));
        CreateMap<RoleId, string>().ConvertUsing(s => s.ToString());

        CreateMap<Role, RoleDataModel>()
            .ForMember(model => model.Meta, expression => expression.MapFrom("_meta"))
            .ForMember(model => model.Permissions, expression => expression.MapFrom("_permissions"))
            .ReverseMap()
            .ForMember(role => role.Meta, expression => expression.Ignore())
            .ForMember(role => role.Permissions, expression => expression.Ignore());
    }
}