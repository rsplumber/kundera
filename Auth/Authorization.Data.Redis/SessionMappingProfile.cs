using Auth.Domain.Sessions;
using AutoMapper;

namespace Authorization.Data.Redis;

internal sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<string, Token>().ConvertUsing(s => Token.From(s));
        CreateMap<Token, string>().ConvertUsing(token => token.Value);

        CreateMap<SessionDataModel, Session>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_refreshToken", expression => expression.MapFrom(model => model.RefreshToken))
            .ForMember("_scope", expression => expression.MapFrom(model => model.Scope))
            .ForMember("_userId", expression => expression.MapFrom(model => model.UserId))
            .ForMember("_expireDate", expression => expression.MapFrom(model => model.ExpireDate))
            .ForMember("_lastUsageDate", expression => expression.MapFrom(model => model.LastUsageDate))
            .ForMember("_lastIpAddress", expression => expression.MapFrom(model => model.LastIpAddress))
            .ReverseMap();
    }
}