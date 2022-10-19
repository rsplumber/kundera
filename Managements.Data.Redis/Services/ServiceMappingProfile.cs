using AutoMapper;
using Managements.Domain.Services;

namespace Managements.Data.Redis.Services;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<ServiceDataModel, Service>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(service => service.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_secret", expression => expression.MapFrom(model => model.Secret))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ReverseMap();
    }
}