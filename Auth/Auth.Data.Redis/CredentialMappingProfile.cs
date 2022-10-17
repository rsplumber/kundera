using Auth.Core.Domains;
using AutoMapper;

namespace Auth.Data.Redis;

internal sealed class CredentialMappingProfile : Profile
{
    public CredentialMappingProfile()
    {
        CreateMap<string, UniqueIdentifier>().ConvertUsing(id => UniqueIdentifier.Parse(id));
        CreateMap<UniqueIdentifier, string>().ConvertUsing(identifier => identifier.Value);

        CreateMap<Password, PasswordType>().ConvertUsing(password => new PasswordType
        {
            Salt = password.Salt,
            Value = password.Value
        });
        CreateMap<PasswordType, Password>().ConvertUsing(passwordType => Password.From(passwordType.Value, passwordType.Salt));

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