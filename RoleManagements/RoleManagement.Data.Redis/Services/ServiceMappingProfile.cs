﻿using AutoMapper;
using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Types;

namespace RoleManagement.Data.Redis.Services;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<string, ServiceId>().ConvertUsing(s => ServiceId.From(s));
        CreateMap<ServiceId, string>().ConvertUsing(s => s.Value);
        
        CreateMap<string, ServiceStatus>().ConvertUsing(s => ServiceStatus.From(s));
        CreateMap<ServiceStatus, string>().ConvertUsing(s => s.Value);

        CreateMap<Service, ServiceDataModel>()
            .ForMember(model => model.Status, expression => expression.MapFrom("_status"))
            .ReverseMap()
            .ForMember(role => role.Status, expression => expression.Ignore());
    }
}