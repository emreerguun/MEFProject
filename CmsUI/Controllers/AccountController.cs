using Microsoft.AspNetCore.Mvc;

namespace CmsUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }
        public class RegisterController : Controller
        {
            private readonly ILogger<RegisterController> _logger;

            public RegisterController(ILogger<RegisterController> logger)
            {
                _logger = logger;
            }

            // Kayıt formunu göster
            public IActionResult Index()
            {
                return View();
            }

            // Kayıt formundan gelen verileri işle
            [HttpPost]
            public IActionResult Index(string username, string email, string password)
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"New user registered: {username} - {email}");
                    // TODO: Kullanıcı kayıt işlemleri (veritabanına kaydet, doğrulama gönder vb.)
                    return RedirectToAction("Login", "Account");
                }

                return View();
            }
        }
    }
}
