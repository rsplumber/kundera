using AutoMapper;
using Core.Domains.Roles;

namespace Managements.Data.Roles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<RoleDataModel, Role>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(role => role.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(role => role.Name, expression => expression.MapFrom(model => model.Name))
            .ForMember(role => role.Meta, expression => expression.MapFrom(model => model.Meta))
            .ForMember(role => role.Permissions, expression => expression.MapFrom(model => model.Permissions))
            .ReverseMap();
    }
}