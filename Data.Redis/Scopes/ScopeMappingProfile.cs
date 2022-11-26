using AutoMapper;
using Core.Domains.Scopes;

namespace Managements.Data.Scopes;

public class ScopeMappingProfile : Profile
{
    public ScopeMappingProfile()
    {
        DisableConstructorMapping();

        CreateMap<ScopeDataModel, Scope>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(scope => scope.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(scope => scope.Name, expression => expression.MapFrom(model => model.Name))
            .ForMember(scope => scope.Secret, expression => expression.MapFrom(model => model.Secret))
            .ForMember(scope => scope.Services, expression => expression.MapFrom(model => model.Services))
            .ForMember(scope => scope.Status, expression => expression.MapFrom(model => model.Status))
            .ForMember(scope => scope.Roles, expression => expression.MapFrom(model => model.Roles))
            .ReverseMap();
    }
}