using AutoMapper;
using Core.Domains.Auth.Credentials;

namespace Managements.Data.Auth.Credentials;

internal sealed class CredentialMappingProfile : Profile
{
    public CredentialMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<CredentialDataModel, Credential>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(credential => credential.Username, expression => expression.MapFrom(model => model.Username))
            .ForMember(credential => credential.User, expression => expression.MapFrom(model => model.UserId))
            .ForMember(credential => credential.Password, expression => expression.MapFrom(model => model.Password))
            .ForMember(credential => credential.LastIpAddress, expression => expression.MapFrom(model => model.LastIpAddress))
            .ForMember(credential => credential.LastLoggedInUtc, expression => expression.MapFrom(model => model.LastLoggedInUtc == null ? model.LastLoggedInUtc : model.LastLoggedInUtc.Value.ToUniversalTime()))
            .ForMember(credential => credential.ExpiresAtUtc, expression => expression.MapFrom(model => model.ExpiresAtUtc == null ? model.ExpiresAtUtc : model.ExpiresAtUtc.Value.ToUniversalTime()))
            .ForMember(credential => credential.CreatedDateUtc, expression => expression.MapFrom(model => model.CreatedDateUtc.ToUniversalTime()))
            .ForMember(credential => credential.OneTime, expression => expression.MapFrom(model => model.OneTime))
            .ForMember(credential => credential.SingleSession, expression => expression.MapFrom(model => model.SingleSession))
            .ForMember(credential => credential.SessionExpireTimeInMinutes, expression => expression.MapFrom(model => model.SessionExpireTimeInMinutes))
            .ReverseMap();
    }
}