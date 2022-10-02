using Authorization.Domain;
using Authorization.Domain.Types;
using AutoMapper;

namespace Authorization.Infrastructure.Data;

internal sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<string, Token>().ConvertUsing(s => Token.From(s));
        CreateMap<Token, string>().ConvertUsing(token => token.Value);

        CreateMap<Session, SessionDataModel>().ReverseMap();
    }
}