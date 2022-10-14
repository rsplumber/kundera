using AutoMapper;
using Domain.Permissions;

namespace Data.Redis.Permissions;

public class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<string, PermissionId>()
            .ConvertUsing(s => PermissionId.From(s));

        CreateMap<PermissionId, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<PermissionDataModel, Permission>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(permission => permission.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_meta", expression => expression.MapFrom(model => model.Meta))
            .ReverseMap();
    }
}