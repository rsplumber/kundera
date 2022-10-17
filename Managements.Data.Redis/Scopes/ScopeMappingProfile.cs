using AutoMapper;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Types;

namespace Managements.Data.Redis.Scopes;

public class ScopeMappingProfile : Profile
{
    public ScopeMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<string, ScopeId>()
            .ConvertUsing(s => ScopeId.From(s));

        CreateMap<ScopeId, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<string, ScopeStatus>()
            .ConvertUsing(s => ScopeStatus.From(s));

        CreateMap<ScopeStatus, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<ScopeDataModel, Scope>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(scope => scope.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_services", expression => expression.MapFrom(model => model.Services))
            .ForMember("_status", expression => expression.MapFrom(model => model.Status))
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ReverseMap();
    }
}