using AutoMapper;
using Managements.Domain.Permissions;

namespace Managements.Data.Permissions;

public class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<PermissionDataModel, Permission>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(permission => permission.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_meta", expression => expression.MapFrom(model => model.Meta))
            .ReverseMap();
    }
}