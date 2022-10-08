using AutoMapper;
using Domain.Services;
using Domain.Services.Types;

namespace Data.Redis.Services;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<string, ServiceId>().ConvertUsing(s => ServiceId.From(s));
        CreateMap<ServiceId, string>().ConvertUsing(s => s.Value);

        CreateMap<string, ServiceStatus>().ConvertUsing(s => ServiceStatus.From(s));
        CreateMap<ServiceStatus, string>().ConvertUsing(s => s.Value);

        CreateMap<ServiceDataModel, Service>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(service => service.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ReverseMap();
    }
}