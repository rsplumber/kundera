using Authorization.Domain;
using AutoMapper;

namespace Authorization.Infrastructure.Data;

internal sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<Session, SessionDataModel>()
            .ForMember(model => model.RefreshToken, expression => expression.MapFrom("_refreshToken"))
            .ForMember(model => model.Scope, expression => expression.MapFrom("_scope"))
            .ForMember(model => model.User, expression => expression.MapFrom("_userId"))
            .ForMember(model => model.ExpireDate, expression => expression.MapFrom("_expireDate"))
            .ForMember(model => model.LastUsageDate, expression => expression.MapFrom("_lastUsageDate"))
            .ForMember(model => model.LastIpAddress, expression => expression.MapFrom("_lastIpAddress"))
            .ReverseMap();
    }
}