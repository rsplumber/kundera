using AutoMapper;
using Core.Domains.Credentials;

namespace Managements.Data.Credentials;

internal sealed class CredentialMappingProfile : Profile
{
    public CredentialMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<CredentialDataModel, Credential>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(credential => credential.User, expression => expression.MapFrom(model => model.UserId))
            .ForMember(credential => credential.Password, expression => expression.MapFrom(model => model.Password))
            .ForMember(credential => credential.LastIpAddress, expression => expression.MapFrom(model => model.LastIpAddress))
            .ForMember(credential => credential.LastLoggedIn, expression => expression.MapFrom(model => model.LastLoggedIn.ToUniversalTime()))
            .ForMember(credential => credential.ExpiresAt, expression => expression.MapFrom(model => model.ExpiresAt == null ? model.ExpiresAt : model.ExpiresAt.Value.ToUniversalTime()))
            .ForMember(credential => credential.CreatedDate, expression => expression.MapFrom(model => model.CreatedDate.ToUniversalTime()))
            .ForMember(credential => credential.OneTime, expression => expression.MapFrom(model => model.OneTime))
            .ReverseMap();
    }
}