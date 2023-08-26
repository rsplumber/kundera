using System.Net;
using Core.Auth.Authorizations.Extensions;
using DotNetCore.CAP;

namespace Core.Auth.Authorizations;

public interface IRoleAuthorizationHandler
{
    ValueTask<AuthorizeResponse> HandleAsync(
        string token,
        string serviceSecret,
        string[] roles,
        IPAddress ipAddress,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}

internal sealed class RoleAuthorizationHandler : IRoleAuthorizationHandler
{
    private readonly IAuthorizeDataProvider _authorizeDataProvider;
    private readonly ICapPublisher _eventBus;

    public RoleAuthorizationHandler(IAuthorizeDataProvider authorizeDataProvider, ICapPublisher eventBus)
    {
        _authorizeDataProvider = authorizeDataProvider;
        _eventBus = eventBus;
    }

    public async ValueTask<AuthorizeResponse> HandleAsync(string token, string serviceSecret, string[] roles, IPAddress ipAddress, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        var session = await _authorizeDataProvider.FindSessionAsync(token, cancellationToken);
        if (!session.Validate(out var sessionValidationFailedResponse))
        {
            return sessionValidationFailedResponse!;
        }

        if (!session!.ValidateUser(out var userValidationFailedResponse))
        {
            return userValidationFailedResponse!;
        }

        var userRoles = await _authorizeDataProvider.UserRolesAsync(session!.User, cancellationToken);
        var service = await _authorizeDataProvider.FindServiceAsync(serviceSecret, cancellationToken);
        if (!session.ValidateScope(service, userRoles, out var scopeValidationFailedResponse))
        {
            return scopeValidationFailedResponse!;
        }

        if (InvalidRole()) return AuthorizeResponse.Forbidden;

        _ = _eventBus.PublishAsync(AuthorizedEvent.EventName, new AuthorizedEvent
        {
            Agent = userAgent,
            IpAddress = ipAddress.ToString(),
            SessionId = session.Id,
            UserId = session.User.Id
        }, cancellationToken: cancellationToken);

        return AuthorizeResponse.Success(session.User.Id, session.Scope.Id, service!.Id);

        bool InvalidRole() => userRoles.All(role => roles.All(r => role.Name != r.ToLower()));
    }
}