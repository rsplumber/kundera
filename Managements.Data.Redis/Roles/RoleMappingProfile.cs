using AutoMapper;
using Managements.Domain.Roles;

namespace Managements.Data.Redis.Roles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<string, RoleId>()
            .ConvertUsing(s => RoleId.From(s));

        CreateMap<RoleId, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<RoleDataModel, Role>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(role => role.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_meta", expression => expression.MapFrom(model => model.Meta))
            .ForMember("_permissions", expression => expression.MapFrom(model => model.Permissions))
            .ReverseMap();
    }
}