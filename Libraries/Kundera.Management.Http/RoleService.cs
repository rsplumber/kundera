using Kundera.Management.Roles;
using RestSharp;

namespace Kundera.Management.Http;

public class RoleService : IRoleService
{
    private readonly ManagementSettings _managementSettings;

    //todo ye response dorost vase hamashoon besaz
    public async ValueTask CreateAsync(string name, IDictionary<string, string>? meta = null, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(_managementSettings.BaseUrl + "/roles")
            .AddJsonBody(new
            {
                name,
                meta
            });
        var client = new RestClient();
        var response = await client.PostAsync(request, cancellationToken);
    }

    public ValueTask DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask AddPermissionsAsync(string id, string[] permissionIds, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask RemovePermissionsAsync(string id, string[] permissionIds, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<RoleResponse> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IEnumerable<RolesResponse>> GetAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}