using AutoMapper;
using Managements.Domain.Services;

namespace Managements.Data.Services;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<ServiceDataModel, Service>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(service => service.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(service => service.Name, expression => expression.MapFrom(model => model.Name))
            .ForMember(service => service.Secret, expression => expression.MapFrom(model => model.Secret))
            .ForMember(service => service.Status, expression => expression.MapFrom(model => model.Status))
            .ReverseMap();
    }
}