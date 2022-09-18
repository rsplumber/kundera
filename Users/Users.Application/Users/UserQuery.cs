using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record UserQuery(UserId UserId) : Query<UserResponse>;

public sealed record UserResponse(string Id)
{
    public string? Firstname { get; set; }
    
    public string? Lastname { get; set; }
    
    public string? Username { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? Email { get; set; }
    
    public string? NationalCode { get; set; }
    
    public List<string> UserGroups { get; set; }
    
    public List<string> Roles { get; set; }
}