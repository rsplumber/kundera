using AutoMapper;
using Managements.Domain.Roles;

namespace Managements.Data.Redis.Roles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<Guid, RoleId>()
            .ConvertUsing(s => RoleId.From(s));

        CreateMap<RoleId, Guid>()
            .ConvertUsing(s => s.Value);

        CreateMap<RoleDataModel, Role>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(role => role.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_meta", expression => expression.MapFrom(model => model.Meta))
            .ForMember("_permissions", expression => expression.MapFrom(model => model.Permissions))
            .ReverseMap();
    }
}