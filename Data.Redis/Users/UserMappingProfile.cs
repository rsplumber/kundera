using AutoMapper;
using Core.Domains;
using Core.Domains.Users;

namespace Managements.Data.Users;

internal sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<string, UserStatus>()
            .ConvertUsing(u => Enumeration.GetAll<UserStatus>().First(status => status.Name == u));

        CreateMap<UserStatus, string>()
            .ConvertUsing(u => u.Name);
        DisableConstructorMapping();
        CreateMap<UserDataModel, User>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(user => user.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(user => user.Usernames, expression => expression.MapFrom(model => model.Usernames))
            .ForMember(user => user.Groups, expression => expression.MapFrom(model => model.Groups))
            .ForMember(user => user.Roles, expression => expression.MapFrom(model => model.Roles))
            .ForMember(user => user.Status, expression => expression.MapFrom(model => model.Status))
            .ForMember(user => user.StatusChangeReason, expression => expression.MapFrom(model => model.StatusChangeReason))
            .ForMember(user => user.StatusChangeDateUtc, expression => expression.MapFrom(model => model.StatusChangeDateUtc))
            .ReverseMap();
    }
}