using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using FastEndpoints;
using KunderaNet.Services.Authorization.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KunderaNet.FastEndpoints.Authorization;

internal sealed class KunderaAuthenticationOption : AuthenticationSchemeOptions
{
}

internal sealed class KunderaAuthenticationHandler : AuthenticationHandler<KunderaAuthenticationOption>
{
    private const string AuthorizationKey = "Authorization";
    private const string UnAuthorizedMessage = "UnAuthorized";
    private const string ForbiddenMessage = "Forbidden";
    private const string SessionExpiredMessage = "Session expired";
    private const string UserTokenKey = "uid_token";
    private readonly IAuthorizeService _authorizeService;
    private static readonly AuthenticationTicket EmptyTicket = new(new ClaimsPrincipal(), string.Empty);
    private static readonly JwtSecurityTokenHandler JwtTokenHandler = new();
    private static readonly byte[] JwtKey = Encoding.ASCII.GetBytes(KunderaAuthorizationSettings.ServiceSecret);

    public KunderaAuthenticationHandler(IOptionsMonitor<KunderaAuthenticationOption> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthorizeService authorizeService) : base(options, logger, encoder, clock)
    {
        _authorizeService = authorizeService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (AllowAnonymous() || IsSwagger())
        {
            return AuthenticateResult.Success(EmptyTicket);
        }

        var userTokenHeader = Context.Request.Headers[UserTokenKey];
        if (userTokenHeader.Count != 0)
        {
            return await AuthorizeByUserTokenAsync(userTokenHeader[0]);
        }

        return await AuthorizeByAuthorizationAsync();

        bool AllowAnonymous() => Context.GetEndpoint()?
            .Metadata
            .GetMetadata<IAllowAnonymous>() != null;

        bool IsSwagger() => Context.Request.Path.ToUriComponent().StartsWith("/swagger");
    }

    private Task<AuthenticateResult> AuthorizeByUserTokenAsync(string? userToken)
    {
        return Task.Run(() =>
        {
            if (string.IsNullOrEmpty(userToken))
            {
                Response.StatusCode = 401;
                return AuthenticateResult.Fail(UnAuthorizedMessage);
            }

            string userId;
            string? scopeId;
            string? serviceId;
            try
            {
                JwtTokenHandler.ValidateToken(userToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(JwtKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                scopeId = jwtToken.Claims.FirstOrDefault(x => x.Type == "scope_id")?.Value;
                serviceId = jwtToken.Claims.FirstOrDefault(x => x.Type == "service_id")?.Value;
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail(UnAuthorizedMessage);
            }

            return AuthenticateResult.Success(CreateAuthenticationTicket(userId, scopeId, serviceId));
        });
    }

    private async Task<AuthenticateResult> AuthorizeByAuthorizationAsync()
    {
        var tokenHeader = Context.Request.Headers[AuthorizationKey];
        if (tokenHeader.Count == 0)
        {
            Response.StatusCode = 401;
            return AuthenticateResult.Fail(UnAuthorizedMessage);
        }

        var tokenValue = tokenHeader[0];
        if (tokenValue is null)
        {
            Response.StatusCode = 401;
            return AuthenticateResult.Fail(UnAuthorizedMessage);
        }

        AuthorizedResponse? authorizedResponse = null;
        var statusCode = 0;
        var allowedPermissions = AllowedPermissions();
        if (allowedPermissions is not null)
        {
            (statusCode, authorizedResponse) = await _authorizeService.AuthorizePermissionAsync(tokenValue, allowedPermissions);
        }
        else
        {
            var allowedRoles = AllowedRoles();
            if (allowedRoles is not null)
            {
                (statusCode, authorizedResponse) = await _authorizeService.AuthorizeRoleAsync(tokenValue, allowedRoles);
            }
        }

        if (authorizedResponse is null)
        {
            return AuthenticateResult.Fail(UnAuthorizedMessage);
        }

        Response.StatusCode = statusCode;
        return statusCode switch
        {
            401 => AuthenticateResult.Fail(UnAuthorizedMessage),
            403 => AuthenticateResult.Fail(ForbiddenMessage),
            440 => AuthenticateResult.Fail(SessionExpiredMessage),
            _ => AuthenticateResult.Success(CreateAuthenticationTicket(authorizedResponse.UserId, authorizedResponse.ScopeId, authorizedResponse.ServiceId))
        };

        EndpointDefinition? GetEndpointDefinition() => (EndpointDefinition?)Context.GetEndpoint()?
            .Metadata
            .FirstOrDefault(o => o is EndpointDefinition);

        List<string>? AllowedPermissions() => GetEndpointDefinition()?.AllowedPermissions;

        List<string>? AllowedRoles() => GetEndpointDefinition()?.AllowedRoles;
    }


    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 401;
        return Task.FromResult(AuthenticateResult.Fail(ForbiddenMessage));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 403;
        return Task.FromResult(AuthenticateResult.Fail(UnAuthorizedMessage));
    }


    private AuthenticationTicket CreateAuthenticationTicket(string userId, string? scopeId, string? serviceId) =>
        new(new ClaimsPrincipal(CreateClaimsIdentity(userId, scopeId, serviceId)), Scheme.Name);

    private static ClaimsIdentity CreateClaimsIdentity(string userId, string? scopeId, string? serviceId) => new(new[]
    {
        new Claim(KunderaDefaults.UserIdKey, userId),
        new Claim(KunderaDefaults.ScopeIdKey, scopeId ?? string.Empty),
        new Claim(KunderaDefaults.ServiceIdKey, serviceId ?? string.Empty)
    }, KunderaDefaults.AuthenticationType);
}