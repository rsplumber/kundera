using Auth.Core;
using AutoMapper;

namespace Auth.Data;

internal sealed class CredentialMappingProfile : Profile
{
    public CredentialMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<CredentialDataModel, Credential>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_userId", expression => expression.MapFrom(model => model.UserId))
            .ForMember("_password", expression => expression.MapFrom(model => model.Password))
            .ForMember("_lastIpAddress", expression => expression.MapFrom(model => model.LastIpAddress))
            .ForMember("_lastLoggedIn", expression => expression.MapFrom(model => model.LastLoggedIn.ToUniversalTime()))
            .ForMember("_expiresAt", expression => expression.MapFrom(model => model.ExpiresAt == null ? model.ExpiresAt : model.ExpiresAt.Value.ToUniversalTime()))
            .ForMember("_createdDate", expression => expression.MapFrom(model => model.CreatedDate.ToUniversalTime()))
            .ForMember("_oneTime", expression => expression.MapFrom(model => model.OneTime))
            .ReverseMap();
    }
}