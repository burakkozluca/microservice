using Microsoft.AspNetCore.Identity;

namespace Services.Identity.Data.Entities;

public class User : IdentityUser<string>
{
    public User()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string? City { get; set; }
    public string? Name { get; set; } 
    public string? Surname { get; set; }

    public DateTime? DateOfBirth { get; set; }
}