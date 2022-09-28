using AutoMapper;
using Users.Domain;
using Users.Domain.Users;
using Users.Domain.Users.Types;

namespace Users.Data.Redis.Users;

internal sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<string, Username>().ConvertUsing(u => Username.From(u));
        CreateMap<Username, string>().ConvertUsing(u => u.Value);

        CreateMap<Guid, UserId>().ConvertUsing(u => UserId.From(u));
        CreateMap<UserId, Guid>().ConvertUsing(u => u.Value);

        CreateMap<string, RoleId>().ConvertUsing(u => RoleId.From(u));
        CreateMap<RoleId, string>().ConvertUsing(u => u.Value);

        CreateMap<string, UserStatus>().ConvertUsing(u => UserStatus.From(u));
        CreateMap<UserStatus, string>().ConvertUsing(u => u.Value);


        CreateMap<User, UserDataModel>()
            .ForMember(model => model.Usernames, expression => expression.MapFrom("_usernames"))
            .ForMember(model => model.UserGroups, expression => expression.MapFrom("_userGroups"))
            .ForMember(model => model.Roles, expression => expression.MapFrom("_roles"))
            .ForMember(model => model.Status, expression => expression.MapFrom("_status"))
            .ForMember(model => model.StatusChangedDate, expression => expression.MapFrom("_statusChangedDate"))
            .ForMember(model => model.StatusChangedReason, expression => expression.MapFrom("_statusChangedReason"))
            .ReverseMap()
            .ForMember(user => user.Usernames, expression => expression.Ignore())
            .ForMember(user => user.Reason, expression => expression.Ignore())
            .ForMember(user => user.Status, expression => expression.Ignore())
            .ForMember(user => user.UserGroups, expression => expression.Ignore())
            .ForMember(user => user.Roles, expression => expression.Ignore());
    }
}