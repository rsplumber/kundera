using AutoMapper;
using Managements.Domain.Services;
using Managements.Domain.Services.Types;

namespace Managements.Data.Redis.Services;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<Guid, ServiceId>()
            .ConvertUsing(s => ServiceId.From(s));

        CreateMap<ServiceId, Guid>()
            .ConvertUsing(s => s.Value);

        CreateMap<string, ServiceStatus>()
            .ConvertUsing(s => ServiceStatus.From(s));

        CreateMap<ServiceStatus, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<ServiceDataModel, Service>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(service => service.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ReverseMap();
    }
}