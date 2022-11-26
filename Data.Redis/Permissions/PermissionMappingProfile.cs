﻿using AutoMapper;
using Core.Domains.Permissions;

namespace Managements.Data.Permissions;

public class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<PermissionDataModel, Permission>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(permission => permission.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(permission => permission.Name, expression => expression.MapFrom(model => model.Name))
            .ForMember(permission => permission.Meta, expression => expression.MapFrom(model => model.Meta))
            .ReverseMap();
    }
}