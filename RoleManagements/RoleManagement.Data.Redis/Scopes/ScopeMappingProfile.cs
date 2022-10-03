using AutoMapper;
using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Types;

namespace RoleManagement.Data.Redis.Scopes;

public class ScopeMappingProfile : Profile
{
    public ScopeMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<string, ScopeStatus>().ConvertUsing(s => ScopeStatus.From(s));
        CreateMap<ScopeStatus, string>().ConvertUsing(s => s.Value);

        CreateMap<string, ScopeId>().ConvertUsing(s => ScopeId.From(s));
        CreateMap<ScopeId, string>().ConvertUsing(s => s.Value);

        CreateMap<Scope, ScopeDataModel>()
            .ReverseMap()
            .ForMember("_services", expression => expression.MapFrom(model => model.Services))
            .ForMember(group => group.Services, expression => expression.Ignore())
            .ForMember("_roles", expression => expression.MapFrom(model => model.Roles))
            .ForMember(group => group.Roles, expression => expression.Ignore());
    }
}