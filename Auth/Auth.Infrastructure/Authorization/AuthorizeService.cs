using System.Net;
using Auth.Application.Authorization;
using Auth.Domain.Sessions;
using Domain.Roles;
using Domain.Scopes;
using Domain.Users;
using Domain.Users.Types;

namespace Authentication.Infrastructure.Authorization;

internal sealed class AuthorizeService : IAuthorizeService
{
    private readonly ISessionManagement _sessionManagement;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScopeRepository _scopeRepository;

    public AuthorizeService(ISessionManagement sessionManagement,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IScopeRepository scopeRepository)
    {
        _sessionManagement = sessionManagement;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _scopeRepository = scopeRepository;
    }

    public async ValueTask AuthorizeAsync(Token token,
        string action,
        string scope,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(token, ipAddress ?? IPAddress.None, cancellationToken);
        if (session is null)
        {
            throw new UnAuthorizedException();
        }

        if (TokenExpired() || InvalidScope())
        {
            throw new UnAuthorizedException();
        }

        var user = await _userRepository.FindAsync(UserId.From(session.UserId), cancellationToken);
        if (user is null || UserIsNotActive())
        {
            throw new UnAuthorizedException();
        }

        var sessionScope = await _scopeRepository.FindAsync(ScopeId.From(session.Scope), cancellationToken);
        if (sessionScope is null || UserHasNotScopeRole())
        {
            throw new UnAuthorizedException();
        }

        var permissions = await FetchPermissionsAsync();

        if (!permissions.Any(action.ToLower().Equals))
        {
            throw new UnAuthorizedException();
        }

        bool TokenExpired() => DateTime.UtcNow >= session.ExpiresAtUtc;

        bool InvalidScope() => !session.Scope.Equals(scope);

        bool UserIsNotActive() => user.Status != UserStatus.Active;

        bool UserHasNotScopeRole() => !user.Roles.Any(id => sessionScope.Has(id));

        async ValueTask<IEnumerable<string>> FetchPermissionsAsync()
        {
            var roles = await _roleRepository.FindAsync(user.Roles.ToArray(), cancellationToken);
            return roles.SelectMany(role => role.Permissions.Select(id => id.Value));
        }
    }
}