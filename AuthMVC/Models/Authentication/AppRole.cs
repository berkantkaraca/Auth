using Microsoft.AspNetCore.Identity;

namespace AuthMVC.Models.Authentication;

/// <summary>
/// Kullanıcının rollerini tanımlayan sınıftır.
/// </summary>
public class AppRole : IdentityRole<int>
{
}
