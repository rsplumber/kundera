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


        CreateMap<Credential, CredentialDataModel>().ReverseMap();
    }
}