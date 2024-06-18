using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backengv2.Data;
using Backengv2.Dtos;
using Backengv2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backengv2.Controllers.Login
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    public class AuthController : ControllerBase // Controlador base para la autenticación
    {
        private readonly BaseContext _context; // Contexto de la base de datos

        public AuthController(BaseContext context)
        {
            _context = context; // Inicializa el contexto de la base de datos
        }

        [HttpPost("Login")] // Define la acción para manejar las solicitudes POST en /api/auth/Login
        public async Task<IActionResult> Login([FromBody] Autorize token)
        {
            var MarketingUser = await _context.MarketingUsers
                .FirstOrDefaultAsync(e => e.Email == token.Email && e.Password == token.Password); // Busca un usuario de marketing por correo y contraseña

            if (MarketingUser == null)
            {
                return BadRequest("Error en Correo o Contraseña"); // Devuelve un error si no se encuentra el usuario
            }

            // Configura la clave secreta y las credenciales de firma
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ncjdncjvurbuedxwn233nnedxee+dfr-"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Crea las reclamaciones del token JWT (en este caso, solo el identificador del nombre)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, MarketingUser.id.ToString())
            };

            // Configura las opciones del token JWT incluyendo emisor, audiencia, reclamaciones, fecha de expiración y credenciales de firma
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5205",
                audience: "https://localhost:5205",
                claims: claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: signinCredentials
            );

            // Genera el token JWT como una cadena de texto
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            // Devuelve el token JWT como respuesta exitosa
            return Ok(new { Token = tokenString });
        }
    }
}
