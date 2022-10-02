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
            .ReverseMap()
            .ForMember("_usernames", expression => expression.MapFrom(model => model.Usernames))
            .ForMember(group => group.Usernames, expression => expression.Ignore())
            .ForMember("_userGroups", expression => expression.MapFrom(model => model.UserGroups))
            .ForMember(group => group.UserGroups, expression => expression.Ignore())
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ForMember(group => group.Roles, expression => expression.Ignore());
    }
}