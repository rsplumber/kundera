using AutoMapper;
using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Types;

namespace RoleManagement.Data.Redis.Services;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<string, ServiceId>().ConvertUsing(s => ServiceId.From(s));
        CreateMap<ServiceId, string>().ConvertUsing(s => s.Value);

        CreateMap<string, ServiceStatus>().ConvertUsing(s => ServiceStatus.From(s));
        CreateMap<ServiceStatus, string>().ConvertUsing(s => s.Value);

        CreateMap<Service, ServiceDataModel>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(service => service.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ReverseMap();
    }
}