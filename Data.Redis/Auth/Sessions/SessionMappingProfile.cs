using AutoMapper;
using Core.Auth.Sessions;

namespace Data.Auth.Sessions;

internal sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<AuthorizationActivity, AuthorizationActivityDataModel>().ConvertUsing(credentialActivity => new AuthorizationActivityDataModel
        {
            Id = credentialActivity.Id,
            CreatedDateUtc = credentialActivity.CreatedDateUtc,
            Agent = credentialActivity.Agent,
            IpAddress = credentialActivity.IpAddress
        });

        CreateMap<AuthorizationActivityDataModel, AuthorizationActivity>().ConvertUsing(authorizationActivityDataModel => new AuthorizationActivity()
        {
            Id = authorizationActivityDataModel.Id,
            IpAddress = authorizationActivityDataModel.IpAddress,
            Agent = authorizationActivityDataModel.Agent,
            Session = authorizationActivityDataModel.Session,
            CreatedDateUtc = authorizationActivityDataModel.CreatedDateUtc,
        });

        DisableConstructorMapping();
        CreateMap<SessionDataModel, Session>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(credential => credential.RefreshToken, expression => expression.MapFrom(model => model.RefreshToken))
            .ForMember(credential => credential.Scope, expression => expression.MapFrom(model => model.ScopeId))
            .ForMember(credential => credential.Credential, expression => expression.MapFrom(model => model.CredentialId))
            .ReverseMap();
    }
}