using AutoMapper;
using Core.Domains.Auth.Sessions;

namespace Data.Auth.Sessions;

internal sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<SessionActivity, SessionActivityDataModel>().ConvertUsing(credentialActivity => new SessionActivityDataModel
        {
            Id = credentialActivity.Id,
            CreatedDateUtc = credentialActivity.CreatedDateUtc,
            Agent = credentialActivity.Agent,
            IpAddress = credentialActivity.IpAddress
        });
        
        CreateMap<SessionActivityDataModel, SessionActivity>().ConvertUsing(credentialActivityDataModel => new SessionActivity()
        {
            Id = credentialActivityDataModel.Id,
            IpAddress = credentialActivityDataModel.IpAddress,
            Agent = credentialActivityDataModel.Agent,
            CreatedDateUtc = credentialActivityDataModel.CreatedDateUtc,
        });
        
        DisableConstructorMapping();
        CreateMap<SessionDataModel, Session>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(credential => credential.RefreshToken, expression => expression.MapFrom(model => model.RefreshToken))
            .ForMember(credential => credential.Scope, expression => expression.MapFrom(model => model.ScopeId))
            .ForMember(credential => credential.Activity, expression => expression.MapFrom(model => model.Activity))
            .ForMember(credential => credential.ExpirationDateUtc, expression => expression.MapFrom(model => model.ExpirationDateUtc.ToUniversalTime()))
            .ForMember(credential => credential.Credential, expression => expression.MapFrom(model => model.CredentialId))
            .ReverseMap();
    }
}