using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PolicyBasedAuthorization.Models.Entities;

namespace PolicyBasedAuthorization.Models.Context;

public class PolicyBasedContext : IdentityDbContext<AppUser, AppRole, int>
{
    public PolicyBasedContext(DbContextOptions options) : base(options) { }
}

