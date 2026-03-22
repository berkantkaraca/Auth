using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyBasedAuthorization.Models;
using System.Diagnostics;

namespace PolicyBasedAuthorization.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Page1()
    {
        return View();
    }

    [Authorize(Policy = "TimeControl")]
    public IActionResult Page2()
    {
        return View();
    }
}
