using AutoMapper;
using Tes.Domain.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Types;

namespace Users.Data.Redis.UserGroups;

internal sealed class UserGroupMappingProfile : Profile
{
    public UserGroupMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<Guid, IIdentity>().ConvertUsing<TypeTypeConverter>();
        CreateMap<string, RoleId>().ConvertUsing<TypeTTypeConverter>();
        CreateMap<string, UserGroupStatus>().ConvertUsing<TypeSTypeConverter>();
        CreateMap<UserGroup, UserGroupDataModel>()
            .ReverseMap()
            .ForPath(group => group.Roles, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ForMember(group => group.Roles, expression => expression.Ignore());
    }

    sealed class TypeTypeConverter : ITypeConverter<Guid, IIdentity>
    {
        public IIdentity Convert(Guid source, IIdentity destination, ResolutionContext context)
        {
            return UserGroupId.From(source);
        }
    }

    sealed class TypeTTypeConverter : ITypeConverter<string, RoleId>
    {
        public RoleId Convert(string source, RoleId destination, ResolutionContext context)
        {
            return RoleId.From(source);
        }
    }
    
    sealed class TypeSTypeConverter : ITypeConverter<string, UserGroupStatus>
    {

        public UserGroupStatus Convert(string source, UserGroupStatus destination, ResolutionContext context)
        {
            return UserGroupStatus.From(source);
        }
    }
}