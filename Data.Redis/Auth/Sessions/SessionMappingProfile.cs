using AutoMapper;
using Core.Domains.Auth.Sessions;

namespace Data.Auth.Sessions;

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
            .ForMember(credential => credential.ScopeId, expression => expression.MapFrom(model => model.ScopeId))
            .ForMember(credential => credential.UserId, expression => expression.MapFrom(model => model.UserId))
            .ForMember(credential => credential.ExpirationDateUtc, expression => expression.MapFrom(model => model.ExpirationDateUtc.ToUniversalTime()))
            .ForMember(credential => credential.CreatedDateUtc, expression => expression.MapFrom(model => model.CreatedDateUtc.ToUniversalTime()))
            .ForMember(credential => credential.LastUsageDateUtc, expression => expression.MapFrom(model => model.LastUsageDateUtc.ToUniversalTime()))
            .ForMember(credential => credential.LastIpAddress, expression => expression.MapFrom(model => model.LastIpAddress))
            .ForMember(credential => credential.CredentialId, expression => expression.MapFrom(model => model.CredentialId))
            .ForMember(credential => credential.UserAgent, expression => expression.MapFrom(model => model.UserAgent))
            .ReverseMap();
    }
}