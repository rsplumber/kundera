using Core.Auth.Sessions;
using Core.Roles;
using Core.Services;
using Core.Users;

namespace Core.Auth.Authorizations.Extensions;

public static class SessionExtensions
{
    internal static bool Validate(this Session? session, out AuthorizeResponse? unAuthorizeResponse)
    {
        if (session is null)
        {
            unAuthorizeResponse = AuthorizeResponse.UnAuthorized;
            return false;
        }

        if (IsSessionExpired())
        {
            unAuthorizeResponse = AuthorizeResponse.SessionExpired;
            return false;
        }

        unAuthorizeResponse = null;
        return true;

        bool IsSessionExpired() => DateTime.UtcNow >= session.TokenExpirationDateUtc;
    }

    internal static bool ValidateUser(this Session session, out AuthorizeResponse? unAuthorizeResponse)
    {
        if (UserIsNotActive())
        {
            unAuthorizeResponse = AuthorizeResponse.UnAuthorized;
            return false;
        }

        unAuthorizeResponse = null;

        return true;
        bool UserIsNotActive() => session.User.Status != UserStatus.Active;
    }

    internal static bool ValidateScope(this Session session, Service? service, List<Role> requestedRoles, out AuthorizeResponse? unAuthorizeResponse)
    {
        if (service is null || requestedRoles.Count == 0)
        {
            unAuthorizeResponse = AuthorizeResponse.UnAuthorized;
            return false;
        }

        if (InvalidSessionScopeService())
        {
            unAuthorizeResponse = AuthorizeResponse.UnAuthorized;
            return false;
        }

        if (UserHasNotScopeRole())
        {
            unAuthorizeResponse = AuthorizeResponse.UnAuthorized;
            return false;
        }


        if (service.Name == EntityBaseValues.KunderaServiceName)
        {
            unAuthorizeResponse = null;
            return true;
        }

        unAuthorizeResponse = null;
        return true;

        bool InvalidSessionScopeService() => session.Scope.Services.All(s => s.Id != service.Id);

        bool UserHasNotScopeRole() => !requestedRoles.Any(role => session.Scope.Roles.Any(r => r.Id == role.Id));
    }
}