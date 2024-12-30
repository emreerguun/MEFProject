using Architecture.DbWorks.Contexts;
using CmsUI.Models;
using CmsUI.ViewModels;
using Core.Domain.Entities.Admin;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CmsUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AccountController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Admin user)
        {
            if (await _context.Admins.AnyAsync(u => u.Email == user.Email))
                return BadRequest("Email already exists.");

            string hashedPassword = HashPassword(user.PasswordHash);
            if (string.IsNullOrEmpty(hashedPassword))
                return BadRequest("Email already exists.");

            user.PasswordHash = hashedPassword;
            _context.Admins.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Admin user)
        {
            var dbUser = await _context.Admins.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (dbUser == null || !VerifyPassword(user.PasswordHash, dbUser.PasswordHash))
            {
                ModelState.AddModelError("", "Kullanıcı Adı veya Şifre Hatalı.");
                return View(user);
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, string.Format("{0} {1}", dbUser.FirstName, dbUser.LastName)), new Claim(ClaimTypes.NameIdentifier, dbUser.Email) };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            var token = _jwtService.GenerateToken(claims);
            Response.Cookies.Append("jwt-admin", token, new CookieOptions { HttpOnly = true, Secure = true });

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt-admin");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _context.Admins.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return BadRequest("Email not found.");

            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            var resetLink = $"https://yourapp.com/reset-password?token={user.ResetToken}";
            await new EmailService().SendEmailAsync(email, "Password Reset", $"Click <a href='{resetLink}'>here</a> to reset your password.");

            return Ok("Password reset email sent.");
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
