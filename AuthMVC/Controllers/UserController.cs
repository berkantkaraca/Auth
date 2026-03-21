using AuthMVC.Models.Authentication;
using AuthMVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthMVC.Controllers;

public class UserController : Controller
{
    readonly UserManager<AppUser> _userManager;
    public UserController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View(_userManager.Users);
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(AppUserViewModel appUserViewModel)
    {
        if (ModelState.IsValid)
        {
            AppUser appUser = new AppUser
            {
                UserName = appUserViewModel.UserName,
                Email = appUserViewModel.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, appUserViewModel.Sifre);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                result.Errors.ToList().ForEach(e => ModelState.AddModelError("", e.Description));
        }
        return View();
    }
}
