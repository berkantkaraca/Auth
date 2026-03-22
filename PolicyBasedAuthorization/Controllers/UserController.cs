using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PolicyBasedAuthorization.Models.Entities;
using PolicyBasedAuthorization.Models.Entities.ViewModels;

namespace PolicyBasedAuthorization.Controllers;

public class UserController : Controller
{
    UserManager<AppUser> _userManager;
    SignInManager<AppUser> _signInManager;
    public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginVM model)
    {
        AppUser user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "User");
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateVM model)
    {
        AppUser user = new AppUser
        {
            Email = model.Email,
            Name = model.Name,
            Surname = model.Surname,
            UserName = model.Email
        };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
            return RedirectToAction("Login", "User");

        return View(model);
    }
    public IActionResult Denied()
    {
        return View();
    }
}

