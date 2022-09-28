using AutoMapper;
using Users.Domain.Users;

namespace Users.Data.Redis.Users;

internal sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDataModel>()
            .ForMember(model => model.Usernames, expression => expression.MapFrom("_usernames"))
            .ForMember(model => model.UserGroups, expression => expression.MapFrom("_userGroups"))
            .ForMember(model => model.Roles, expression => expression.MapFrom("_roles"))
            .ForMember(model => model.Status, expression => expression.MapFrom("_status"))
            .ForMember(model => model.StatusChangedDate, expression => expression.MapFrom("_statusChangedDate"))
            .ForMember(model => model.StatusChangedReason, expression => expression.MapFrom("_statusChangedReason"));
    }
}