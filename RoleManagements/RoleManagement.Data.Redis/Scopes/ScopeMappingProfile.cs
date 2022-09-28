using AutoMapper;
using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Types;

namespace RoleManagement.Data.Redis.Scopes;

public class ScopeMappingProfile : Profile
{
    public ScopeMappingProfile()
    {
        CreateMap<string, ScopeStatus>().ConvertUsing(s => ScopeStatus.From(s));
        CreateMap<ScopeStatus, string>().ConvertUsing(s => s.ToString());

        CreateMap<string, ScopeId>().ConvertUsing(s => ScopeId.From(s));
        CreateMap<ScopeId, string>().ConvertUsing(s => s.ToString());

        CreateMap<Scope, ScopeDataModel>()
            .ForMember(model => model.Status, expression => expression.MapFrom("_status"))
            .ForMember(model => model.Services, expression => expression.MapFrom("_services"))
            .ForMember(model => model.Roles, expression => expression.MapFrom("_roles"))
            .ReverseMap()
            .ForMember(scope => scope.Services, expression => expression.Ignore())
            .ForMember(scope => scope.Roles, expression => expression.Ignore())
            .ForMember(scope => scope.Status, expression => expression.Ignore());
    }
}