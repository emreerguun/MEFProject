using Architecture.DbWorks.Contexts;
using CmsUI.Models;
using CmsUI.ViewModels;
using Core.Domain.Entities.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace CmsUI.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
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
        //[HttpPost("register")]
        [HttpPost()]
        public async Task<IActionResult> Register(Admin user)
        {
            if (await _context.Admins.AnyAsync(u => u.Email == user.Email))
                return BadRequest("Email already exists.");

            user.PasswordHash = HashPassword(user.PasswordHash);
            _context.Admins.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        //[HttpPost("login")]
        [HttpPost()]
        public async Task<IActionResult> Login(Admin user)
        {
            var dbUser = await _context.Admins.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (dbUser == null || !VerifyPassword(user.PasswordHash, dbUser.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _jwtService.GenerateToken(dbUser.FirstName+"-"+dbUser.LastName);
            return RedirectToAction("Home", "Index", new { Token = token });
        }

        //[HttpPost("forgot-password")]
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
