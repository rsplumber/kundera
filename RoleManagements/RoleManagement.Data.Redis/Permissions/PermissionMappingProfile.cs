using AutoMapper;
using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Types;

namespace RoleManagement.Data.Redis.Permissions;

public class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        CreateMap<string, PermissionId>().ConvertUsing(s => PermissionId.From(s));
        CreateMap<PermissionId, string>().ConvertUsing(s => s.ToString());

        CreateMap<Permission, PermissionDataModel>()
            .ForMember(model => model.Meta, expression => expression.MapFrom("_meta"))
            .ReverseMap()
            .ForMember(permission => permission.Meta, expression => expression.Ignore());
    }
}