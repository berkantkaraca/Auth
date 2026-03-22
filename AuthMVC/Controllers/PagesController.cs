using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMVC.Controllers
{
    [Authorize()]
    public class PagesController : Controller
    {
        [Authorize(Roles = "Editor")]
        public IActionResult Editor()
        {
            return View();
        }
        [Authorize(Roles = "Moderator")]
        public IActionResult Moderator()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }
    }
}
