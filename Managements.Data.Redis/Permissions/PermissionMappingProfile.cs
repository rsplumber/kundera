using AutoMapper;
using Managements.Domain.Permissions;

namespace Managements.Data.Redis.Permissions;

public class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<Guid, PermissionId>()
            .ConvertUsing(s => PermissionId.From(s));

        CreateMap<PermissionId, Guid>()
            .ConvertUsing(s => s.Value);

        CreateMap<PermissionDataModel, Permission>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(permission => permission.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_meta", expression => expression.MapFrom(model => model.Meta))
            .ReverseMap();
    }
}