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

        CreateMap<Session, SessionDataModel>()
            .ForMember(model => model.RefreshToken, expression => expression.MapFrom("_refreshToken"))
            .ForMember(model => model.Scope, expression => expression.MapFrom("_scope"))
            .ForMember(model => model.User, expression => expression.MapFrom("_userId"))
            .ForMember(model => model.ExpireDate, expression => expression.MapFrom("_expireDate"))
            .ForMember(model => model.LastUsageDate, expression => expression.MapFrom("_lastUsageDate"))
            .ForMember(model => model.LastIpAddress, expression => expression.MapFrom("_lastIpAddress"))
            .ReverseMap()
            .ForMember(session => session.Scope, expression => expression.Ignore())
            .ForMember(session => session.ExpireDate, expression => expression.Ignore())
            .ForMember(session => session.RefreshToken, expression => expression.Ignore())
            .ForMember(session => session.UserId, expression => expression.Ignore())
            .ForMember(session => session.LastIpAddress, expression => expression.Ignore())
            .ForMember(session => session.LastUsageDate, expression => expression.Ignore());
    }
}