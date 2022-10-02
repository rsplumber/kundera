using AutoMapper;
using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Types;

namespace RoleManagement.Data.Redis.Permissions;

public class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        CreateMap<string, PermissionId>().ConvertUsing(s => PermissionId.From(s));
        CreateMap<PermissionId, string>().ConvertUsing(s => s.Value);

        CreateMap<Permission, PermissionDataModel>()
            .ReverseMap()
            .ForMember("_meta", expression => expression.MapFrom(model => model.Meta))
            .ForMember(group => group.Meta, expression => expression.Ignore());
        
    }
}