using Authentication.Domain;
using Authentication.Domain.Types;
using AutoMapper;

namespace Authentication.Infrastructure.Data;

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


        CreateMap<Credential, CredentialDataModel>()
            .ForMember(model => model.UserId, expression => expression.MapFrom("_userId"))
            .ForMember(model => model.Password, expression => expression.MapFrom("_password"))
            .ForMember(model => model.LastIpAddress, expression => expression.MapFrom("_lastIpAddress"))
            .ForMember(model => model.LastLoggedIn, expression => expression.MapFrom("_lastLoggedIn"))
            .ForMember(model => model.ExpiresAt, expression => expression.MapFrom("_expiresAt"))
            .ForMember(model => model.OneTime, expression => expression.MapFrom("_oneTime"))
            .ReverseMap()
            .ForMember(credential => credential.User, expression => expression.Ignore())
            .ForMember(credential => credential.Password, expression => expression.Ignore())
            .ForMember(credential => credential.ExpiresAt, expression => expression.Ignore())
            .ForMember(credential => credential.OneTime, expression => expression.Ignore())
            .ForMember(credential => credential.LastIpAddress, expression => expression.Ignore())
            .ForMember(credential => credential.LastLoggedIn, expression => expression.Ignore());
    }
}