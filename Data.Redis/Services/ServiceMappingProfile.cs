using AutoMapper;
using Core.Domains;
using Core.Domains.Services;

namespace Data.Services;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<string, ServiceStatus>()
            .ConvertUsing(s => Enumeration.GetAll<ServiceStatus>().First(status => status.Name == s));

        CreateMap<ServiceStatus, string>()
            .ConvertUsing(s => s.Name);
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