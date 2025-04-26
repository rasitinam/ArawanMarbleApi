using ArawanMarbleApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArawanMarbleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // ASP.NET Core'da JWT Token oluşturma örneği
    public class AuthController : ControllerBase
    {
        private readonly Ara56nmarblecomContext _context;

        public AuthController(Ara56nmarblecomContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);

            if (user == null)
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }

            // Token oluştur
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = "YourSecretKeyAtLeast16CharactersLong"; // Burada gizli anahtarınızı kullanın

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        // Diğer claim'ler ekleyebilirsiniz
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
