using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContApi.Models;
using GestionContactosApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GestionContactosApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly DbA358b2Pam3Context _context;

        public UsuariosController(DbA358b2Pam3Context context)
        {
            _context = context;
        }

        // POST: api/Usuarios/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            var existe = await _context.Usuario.AnyAsync(u => u.Email == usuario.Email);
            if (existe)
                return BadRequest("El email ya está registrado.");

            usuario.Activo = true;

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado correctamente." });
        }
        // POST: api/Usuarios/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest login, [FromServices] IConfiguration config)
        {
            var usuario = await _context.Usuario
                .Where(u => u.Activo == true &&
                           (u.Email == login.UserName || u.NombreApellido == login.UserName) &&
                           u.Password == login.Password)
                .FirstOrDefaultAsync();

            if (usuario == null)
                return Unauthorized("Credenciales inválidas.");

            var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings faltante");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, usuario.NombreApellido ?? usuario.Email ?? ""),
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpireMinutes),
                signingCredentials: creds
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenStr });
        }

    }
}
