using Microsoft.AspNetCore.Mvc;
using WhatsAppMultiBackend.Models;
using WhatsAppMultiBackend.Services;
using WhatsAppMultiBackend.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace WhatsAppMultiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public AuthController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Username already exists.");

            request.PasswordHash = HashPassword(request.PasswordHash);

            if (request.Role == "Client")
                _context.Clients.Add(new Client { Username = request.Username, PasswordHash = request.PasswordHash, Role = "Client" });
            else if (request.Role == "Employee")
                _context.Employees.Add(new Employee { Username = request.Username, PasswordHash = request.PasswordHash, Role = "Employee" });
            else
                return BadRequest("Invalid role.");

            await _context.SaveChangesAsync();
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !VerifyPassword(request.PasswordHash, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return storedHash == enteredHash;
        }
    }
}
