using System.Net;
using Core.Auth.Authorizations.Extensions;
using Core.Auth.Sessions;
using Core.Hashing;
using DotNetCore.CAP;

namespace Core.Auth.Authorizations.Handlers;

internal class RoleAuthorizationHandler : IRoleAuthorizationHandler
{
    private readonly IAuthorizeDataProvider _authorizeDataProvider;
    private readonly ICapPublisher _eventBus;
    private readonly IHashService _hashService;


    public RoleAuthorizationHandler(IAuthorizeDataProvider authorizeDataProvider, ICapPublisher eventBus, IHashService hashService)
    {
        _authorizeDataProvider = authorizeDataProvider;
        _eventBus = eventBus;
        _hashService = hashService;
    }

    public virtual async ValueTask<AuthorizeResponse> HandleAsync(string token, string serviceSecret, IEnumerable<string> roles, IPAddress ipAddress, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        var hashedToken = await _hashService.HashAsync(Session.StaticHashKey, token);
        var session = await _authorizeDataProvider.FindSessionAsync(hashedToken, cancellationToken);
        if (!session.Validate(out var sessionValidationFailedResponse))
        {
            return sessionValidationFailedResponse!;
        }

        if (!session!.ValidateUser(out var userValidationFailedResponse))
        {
            return userValidationFailedResponse!;
        }

        var userRoles = await _authorizeDataProvider.FindUserRolesAsync(session!.User, cancellationToken);
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
            UserId = session.User.Id,
        }, cancellationToken: cancellationToken);

        return AuthorizeResponse.Success(session.User.Id, session.Scope.Id, service!.Id);

        // bool HasNotRole() => !userRoles.Any(role => roles.Any(r => role.Name.Equals(r, StringComparison.CurrentCultureIgnoreCase))); 
        bool InvalidRole() => userRoles.All(role => roles.All(r => !role.Name.Equals(r, StringComparison.CurrentCultureIgnoreCase)));
    }
}