using AutoMapper;
using Managements.Domain.Users;

namespace Managements.Data.Users;

internal sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<UserDataModel, User>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(user => user.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_usernames", expression => expression.MapFrom(model => model.Usernames))
            .ForMember("_userGroups", expression => expression.MapFrom(model => model.UserGroups))
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ForMember("_statusChangedReason", expression => expression.MapFrom(model => model.StatusChangedReason))
            .ForMember("_statusChangedDate", expression => expression.MapFrom(model => model.StatusChangedDate.ToUniversalTime()))
            .ReverseMap();
    }
}