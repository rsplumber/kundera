using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record UsersQuery() : Query<IEnumerable<UserResponse>>;

public sealed record UsersResponse(string Id)
{
    
    public string? Firstname { get; set; }
    
    public string? Lastname { get; set; }
    
    public string? Username { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? Email { get; set; }
    
    public string? NationalCode { get; set; }

}