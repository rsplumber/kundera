using AutoMapper;
using Managements.Domain.UserGroups;

namespace Managements.Data.UserGroups;

internal sealed class UserGroupMappingProfile : Profile
{
    public UserGroupMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<UserGroupDataModel, UserGroup>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(group => group.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_description", expression => expression.MapFrom(model => model.Description))
            .ForMember("_parent", expression => expression.MapFrom(model => model.Parent))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ForMember("_statusChangedDate", expression => expression.MapFrom(model => model.StatusChangedDate.ToUniversalTime()))
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ReverseMap();
    }
}