using Core.Auth.Credentials;
using Core.Auth.Credentials.Exceptions;
using FastEndpoints;

namespace Application.Users.CredentialValidate.Internal;

file sealed class Endpoint : Endpoint<Request>
{
    private readonly ICredentialRepository _credentialRepository;

    public Endpoint(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public override void Configure()
    {
        Post("internal/users/credential/validate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var credentials = await _credentialRepository.FindByUsernameAsync(req.Username, ct);
        var credential = credentials.FirstOrDefault(credential => credential.Username == req.Username && credential.Password.Check(req.Password));
        if (credential is null) throw new WrongUsernamePasswordException();
        await SendOkAsync(ct);
    }
}

file sealed record Request
{
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new user in the system";
        Description = "Create a new user in the system";
        Response(201, "User was successfully created");
    }
}