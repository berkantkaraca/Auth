using Microsoft.AspNetCore.Identity;

namespace PolicyBasedAuthorization.Models.Entities;

public class AppUser : IdentityUser<int>
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

