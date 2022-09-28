using AutoMapper;
using Users.Domain.UserGroups;

namespace Users.Data.Redis.UserGroups;

internal sealed class UserGroupMappingProfile : Profile
{
    public UserGroupMappingProfile()
    {
        CreateMap<UserGroup, UserGroupDataModel>()
            .ForMember(model => model.Name, expression => expression.MapFrom("_name"))
            .ForMember(model => model.Description, expression => expression.MapFrom("_description"))
            .ForMember(model => model.Parent, expression => expression.MapFrom("_parent"))
            .ForMember(model => model.Status, expression => expression.MapFrom("_status"))
            .ForMember(model => model.StatusChangedDate, expression => expression.MapFrom("_statusChangedDate"))
            .ForMember(model => model.Roles, expression => expression.MapFrom("_roles"));
    }
}