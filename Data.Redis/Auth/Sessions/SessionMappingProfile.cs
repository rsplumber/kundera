using AutoMapper;
using Core.Domains.Auth.Sessions;

namespace Managements.Data.Auth.Sessions;

internal sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<SessionDataModel, Session>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(credential => credential.RefreshToken, expression => expression.MapFrom(model => model.RefreshToken))
            .ForMember(credential => credential.Scope, expression => expression.MapFrom(model => model.ScopeId))
            .ForMember(credential => credential.User, expression => expression.MapFrom(model => model.UserId))
            .ForMember(credential => credential.ExpirationDateUtc, expression => expression.MapFrom(model => model.ExpiresAt.ToUniversalTime()))
            .ForMember(credential => credential.CreatedDateUtc, expression => expression.MapFrom(model => model.CreatedDate.ToUniversalTime()))
            .ForMember(credential => credential.LastUsageDateUtc, expression => expression.MapFrom(model => model.LastUsageDate.ToUniversalTime()))
            .ForMember(credential => credential.LastIpAddress, expression => expression.MapFrom(model => model.LastIpAddress))
            .ForMember(credential => credential.Credential, expression => expression.MapFrom(model => model.CredentialId))
            .ReverseMap();
    }
}