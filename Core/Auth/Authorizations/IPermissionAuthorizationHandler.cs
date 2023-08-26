using System.Net;
using Core.Auth.Authorizations.Extensions;
using DotNetCore.CAP;

namespace Core.Auth.Authorizations;

public interface IPermissionAuthorizationHandler
{
    ValueTask<AuthorizeResponse> HandleAsync(
        string token,
        string serviceSecret,
        string[] actions,
        IPAddress ipAddress,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}

internal sealed class PermissionAuthorizationHandler : IPermissionAuthorizationHandler
{
    private readonly IAuthorizeDataProvider _authorizeDataProvider;
    private readonly ICapPublisher _eventBus;

    public PermissionAuthorizationHandler(IAuthorizeDataProvider authorizeDataProvider, ICapPublisher eventBus)
    {
        _authorizeDataProvider = authorizeDataProvider;
        _eventBus = eventBus;
    }

    public async ValueTask<AuthorizeResponse> HandleAsync(string token, string serviceSecret, string[] actions, IPAddress ipAddress, string? userAgent = null, CancellationToken cancellationToken = default)
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

        if (InvalidPermission()) return AuthorizeResponse.Forbidden;

        _ = _eventBus.PublishAsync(AuthorizedEvent.EventName, new AuthorizedEvent
        {
            Agent = userAgent,
            IpAddress = ipAddress.ToString(),
            SessionId = session.Id,
            UserId = session.User.Id
        }, cancellationToken: cancellationToken);


        return AuthorizeResponse.Success(session.User.Id, session.Scope.Id, service!.Id);

        bool InvalidPermission() => userRoles.SelectMany(role => role.Permissions)
            .All(permission => actions.All(action => permission.Name != action.ToLower()));
    }
}