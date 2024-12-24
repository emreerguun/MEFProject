using CmsUI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CmsUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
