﻿using AutoMapper;
using Users.Domain;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Types;

namespace Users.Data.Redis.UserGroups;

internal sealed class UserGroupMappingProfile : Profile
{
    public UserGroupMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<Guid, UserGroupId>().ConvertUsing(u => UserGroupId.From(u));
        CreateMap<UserGroupId, Guid>().ConvertUsing(u => u.Value);

        CreateMap<string, RoleId>().ConvertUsing(u => RoleId.From(u));
        CreateMap<RoleId, string>().ConvertUsing(u => u.Value);

        CreateMap<string, UserGroupStatus>().ConvertUsing(u => UserGroupStatus.From(u));
        CreateMap<UserGroupStatus, string>().ConvertUsing(u => u.Value);
        
        CreateMap<UserGroupDataModel, UserGroup>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(group => group.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_description", expression => expression.MapFrom(model => model.Description))
            .ForMember("_parent", expression => expression.MapFrom(model => model.Parent))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ForMember("_statusChangedDate", expression => expression.MapFrom(model => model.StatusChangedDate))
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ReverseMap();
        

    }
}