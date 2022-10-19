using Auth.Core;
using AutoMapper;

namespace Auth.Data.Redis;

internal sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        DisableConstructorMapping();
        CreateMap<SessionDataModel, Session>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember("_refreshToken", expression => expression.MapFrom(model => model.RefreshToken))
            .ForMember("_scopeId", expression => expression.MapFrom(model => model.ScopeId))
            .ForMember("_userId", expression => expression.MapFrom(model => model.UserId))
            .ForMember("_expiresAt", expression => expression.MapFrom(model => model.ExpiresAt.ToUniversalTime()))
            .ForMember("_createdDate", expression => expression.MapFrom(model => model.CreatedDate.ToUniversalTime()))
            .ForMember("_lastUsageDate", expression => expression.MapFrom(model => model.LastUsageDate.ToUniversalTime()))
            .ForMember("_lastIpAddress", expression => expression.MapFrom(model => model.LastIpAddress))
            .ReverseMap();
    }
}