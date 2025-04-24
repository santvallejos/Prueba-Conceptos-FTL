using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pruebas_Conceptos_MVC_FTG.Model.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Pruebas_Conceptos_MVC_FTG.Data;
using Microsoft.Extensions.Configuration;

namespace Pruebas_Conceptos_MVC_FTG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Pruebas_Conceptos_MVC_FTG_DbContext _context;
        private readonly IConfiguration _configuration;


        // Constructor con inyección de dependencias
        public AuthController(Pruebas_Conceptos_MVC_FTG_DbContext context, IConfiguration configuration)
        {
            _context = context;  // Inyectando el DbContext
            _configuration = configuration;
        }

        ///<summary>
        /// Endpoint para login y generación de JWT
        ///</summary>

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == login.Email);
            if (usuario == null || !usuario.VerificarContrasena(login.Password))
            {
                return Unauthorized("Por favor, proporcione un nombre de usuario y una contraseña válidos.");
            }

            // Verificar que la clave JWT no sea nula
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                return StatusCode(500, "Error: La clave JWT no está configurada correctamente.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(key);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                     [
                        new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim("UsuarioId", usuario.Id.ToString())
                    ]),
                Expires = DateTime.UtcNow.AddHours(8), //como horario laboral, pero se puede cambiar
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }


    }
}