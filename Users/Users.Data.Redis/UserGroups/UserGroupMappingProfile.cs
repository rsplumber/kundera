using AutoMapper;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Types;

namespace Users.Data.Redis.UserGroups;

internal sealed class UserGroupMappingProfile : Profile
{
    public UserGroupMappingProfile()
    {
        CreateMap<Guid, UserGroupId>().ConvertUsing(u => UserGroupId.From(u));
        CreateMap<UserGroupId, Guid>().ConvertUsing(u => u.Value);

        CreateMap<string, UserGroupStatus>().ConvertUsing(u => UserGroupStatus.From(u));
        CreateMap<UserGroupStatus, string>().ConvertUsing(u => u.Value);


        CreateMap<UserGroup, UserGroupDataModel>()
            .ForMember(model => model.Name, expression => expression.MapFrom("_name"))
            .ForMember(model => model.Description, expression => expression.MapFrom("_description"))
            .ForMember(model => model.Parent, expression => expression.MapFrom("_parent"))
            .ForMember(model => model.Status, expression => expression.MapFrom("_status"))
            .ForMember(model => model.StatusChangedDate, expression => expression.MapFrom("_statusChangedDate"))
            .ForMember(model => model.Roles, expression => expression.MapFrom("_roles"))
            .ReverseMap()
            .ForMember(group => group.Description, expression => expression.Ignore())
            .ForMember(group => group.Name, expression => expression.Ignore())
            .ForMember(group => group.Parent, expression => expression.Ignore())
            .ForMember(group => group.Roles, expression => expression.Ignore())
            .ForMember(group => group.StatusChangedDate, expression => expression.Ignore())
            .ForMember(group => group.UserGroupStatus, expression => expression.Ignore());
    }
}