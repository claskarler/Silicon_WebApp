using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entitites;

public class UserEntity : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Biography { get; set; }
    public string? ProfileImage { get; set; } = "avatar.jpg";
    public int? AddressId { get; set; }
    public AddressEntity? Address { get; set; }
}
