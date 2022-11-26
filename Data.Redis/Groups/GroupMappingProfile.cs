using AutoMapper;
using Core.Domains.Groups;

namespace Managements.Data.Groups;

internal sealed class GroupMappingProfile : Profile
{
    public GroupMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<GroupDataModel, Group>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(group => group.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(group => group.Name, expression => expression.MapFrom(model => model.Name))
            .ForMember(group => group.Description, expression => expression.MapFrom(model => model.Description))
            .ForMember(group => group.Parent, expression => expression.MapFrom(model => model.Parent))
            .ForMember(group => group.Status, expression => expression.MapFrom(model => model.Status))
            .ForMember(group => group.StatusChangeDate, expression => expression.MapFrom(model => model.StatusChangeDate.ToUniversalTime()))
            .ForMember(group => group.Roles, expression => expression.MapFrom(model => model.Roles))
            .ForMember(group => group.Children, expression => expression.MapFrom(model => model.Children))
            .ReverseMap();
    }
}