using AuthMVC.Models.Authentication;
using AuthMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthMVC.Controllers;

public class UserController : Controller
{
    readonly UserManager<AppUser> _userManager;
    readonly SignInManager<AppUser> _signInManager;
    public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View(_userManager.Users);
    }

    public IActionResult Login(string ReturnUrl)
    {
        TempData["returnUrl"] = ReturnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                //İlgili kullanıcıya dair önceden oluşturulmuş bir Cookie varsa siliyoruz.
                await _signInManager.SignOutAsync();
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, model.Persistent, model.Lock);

                if (result.Succeeded)
                    return Redirect(TempData["returnUrl"].ToString());
            }
            else
            {
                ModelState.AddModelError("NotUser", "Böyle bir kullanıcı bulunmamaktadır.");
                ModelState.AddModelError("NotUser2", "E-posta v0eya şifre yanlış.");
            }
        }

        return View(model);
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

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }
}
