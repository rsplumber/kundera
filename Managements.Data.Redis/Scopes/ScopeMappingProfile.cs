using AutoMapper;
using Managements.Domain.Scopes;

namespace Managements.Data.Redis.Scopes;

public class ScopeMappingProfile : Profile
{
    public ScopeMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<ScopeDataModel, Scope>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(scope => scope.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_name", expression => expression.MapFrom(model => model.Name))
            .ForMember("_secret", expression => expression.MapFrom(model => model.Secret))
            .ForMember("_services", expression => expression.MapFrom(model => model.Services))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ReverseMap();
    }
}