using FerreteriaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FerreteriaAPI.Models;



namespace FerreteriaAPI.Controllers

{
    [ApiController]
    [Route("api/[controller]")]


    public class AuthController : ControllerBase
    {
        private readonly FerreteriaContext _context;
        private readonly IMemoryCache _cache;

        public AuthController(FerreteriaContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

       

      

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Empleados
                .FirstOrDefaultAsync(e => e.Nombre == request.Usuario);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.Contrasena))
            {
                return Unauthorized("Credenciales inválidas");
            }


            _cache.Set($"session_{usuario.Id}", usuario, TimeSpan.FromMinutes(20));


            var claims = new[]
            {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    "qarYGQHjdYsch42JiItfKfrPwyg9GxhUaOWo7HAGHjw0-wBIYa75eLgbuuBEpq9J"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "ferreteriaapi",
                audience: "ferreteriaapi",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpGet("session/{id}")]
        public IActionResult VerificarSesion(int id)
        {
            if (_cache.TryGetValue($"session_{id}", out Empleado usuario))
            {
                return Ok(new { activo = true, usuario = usuario.Nombre });
            }

            return NotFound(new { activo = false });
        }
 
    }



    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }
}
