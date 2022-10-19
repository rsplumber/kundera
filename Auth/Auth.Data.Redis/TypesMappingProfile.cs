using System.Net;
using Auth.Core;
using AutoMapper;

namespace Auth.Data.Redis;

internal sealed class TypesMappingProfile : Profile
{
    public TypesMappingProfile()
    {
        CreateMap<string, Token>().ConvertUsing(s => Token.From(s));
        CreateMap<Token, string>().ConvertUsing(token => token.Value);
        
        CreateMap<string, UniqueIdentifier>().ConvertUsing(id => UniqueIdentifier.Parse(id));
        CreateMap<UniqueIdentifier, string>().ConvertUsing(identifier => identifier.Value);

        CreateMap<Password, PasswordType>().ConvertUsing(password => new PasswordType
        {
            Salt = password.Salt,
            Value = password.Value
        });
        CreateMap<PasswordType, Password>().ConvertUsing(passwordType => Password.From(passwordType.Value, passwordType.Salt));
        
        CreateMap<string, IPAddress>().ConvertUsing(s => IPAddress.Parse(s));
        CreateMap<IPAddress, string>().ConvertUsing(ip => ip.ToString());
    }
}