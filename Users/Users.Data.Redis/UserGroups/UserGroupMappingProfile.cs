using AutoMapper;
using Users.Domain;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Types;

namespace Users.Data.Redis.UserGroups;

internal sealed class UserGroupMappingProfile : Profile
{
    public UserGroupMappingProfile()
    {
        CreateMap<Guid, UserGroupId>().ConvertUsing(u => UserGroupId.From(u));
        CreateMap<UserGroupId, Guid>().ConvertUsing(u => u.Value);

        CreateMap<string, RoleId>().ConvertUsing(u => RoleId.From(u));
        CreateMap<RoleId, string>().ConvertUsing(u => u.Value);

        CreateMap<string, UserGroupStatus>().ConvertUsing(u => UserGroupStatus.From(u));
        CreateMap<UserGroupStatus, string>().ConvertUsing(u => u.Value);


        CreateMap<UserGroup, UserGroupDataModel>()
            .ReverseMap()
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ForMember(group => group.Roles, expression => expression.Ignore());
    }
}