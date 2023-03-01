using AutoMapper;
using Core.Domains;
using Core.Domains.Groups;

namespace Data.Groups;

internal sealed class GroupMappingProfile : Profile
{
    public GroupMappingProfile()
    {
        CreateMap<string, GroupStatus>()
            .ConvertUsing(u => Enumeration.GetAll<GroupStatus>().First(status => status.Name == u));

        CreateMap<GroupStatus, string>()
            .ConvertUsing(u => u.Name);

        DisableConstructorMapping();
        CreateMap<GroupDataModel, Group>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(group => group.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(group => group.Name, expression => expression.MapFrom(model => model.Name))
            .ForMember(group => group.Description, expression => expression.MapFrom(model => model.Description))
            .ForMember(group => group.ParentId, expression => expression.MapFrom(model => model.Parent))
            .ForMember(group => group.Status, expression => expression.MapFrom(model => model.Status))
            .ForMember(group => group.StatusChangeDateUtc, expression => expression.MapFrom(model => model.StatusChangeDateUtc))
            .ForMember(group => group.Roles, expression => expression.MapFrom(model => model.Roles))
            .ForMember(group => group.Children, expression => expression.MapFrom(model => model.Children))
            .ReverseMap();
    }
}