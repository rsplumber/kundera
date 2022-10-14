using AutoMapper;
using Domain.Roles;
using Domain.Users;
using Domain.Users.Types;

namespace Data.Redis.Users;

internal sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<Guid, UserId>()
            .ConvertUsing(u => UserId.From(u));

        CreateMap<UserId, Guid>()
            .ConvertUsing(u => u.Value);

        CreateMap<string, Username>()
            .ConvertUsing(u => Username.From(u));

        CreateMap<Username, string>()
            .ConvertUsing(u => u.Value);

        CreateMap<string, RoleId>()
            .ConvertUsing(u => RoleId.From(u));

        CreateMap<RoleId, string>()
            .ConvertUsing(u => u.Value);

        CreateMap<string, UserStatus>()
            .ConvertUsing(u => UserStatus.From(u));

        CreateMap<UserStatus, string>()
            .ConvertUsing(u => u.Value);

        CreateMap<UserDataModel, User>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(user => user.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_usernames", expression => expression.MapFrom(model => model.Usernames))
            .ForMember("_userGroups", expression => expression.MapFrom(model => model.UserGroups))
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ForMember("_statusChangedReason", expression => expression.MapFrom(model => model.StatusChangedReason))
            .ForMember("_statusChangedDate", expression => expression.MapFrom(model => model.StatusChangedDate))
            .ReverseMap();
    }
}