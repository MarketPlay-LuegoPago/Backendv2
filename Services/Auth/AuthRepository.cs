using Backengv2.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using BCrypt.Net;
using Backengv2.Data;

namespace Backend.Services
{
    // Clase que implementa la interfaz IAuthRepository para manejar la autenticación y generación de tokens JWT
    public class AuthRepository : IAuthRepository
    {
        private readonly BaseContext _context;
        private readonly string _jwtSecret;

        // Constructor que recibe el contexto de la base de datos y el secreto JWT
        public AuthRepository(BaseContext context, string jwtSecret)
        {
            _context = context;
            _jwtSecret = jwtSecret;
        }

        // Método para verificar si la contraseña coincide con el hash almacenado
        public bool VeriryPassword(string Password, string HashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(Password, HashedPassword);
        }

        // Método para generar un token JWT para el usuario proporcionado
        public string GenerateToken(MarketingUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenOptions = new JwtSecurityToken(
                issuer: @Environment.GetEnvironmentVariable("jwtUrl"),
                audience: @Environment.GetEnvironmentVariable("jwtUrl"),
                claims: new List<Claim>(),
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            // Genera el token JWT
            return tokenHandler.WriteToken(tokenOptions);
        }
    }

    // Clase vacía que probablemente debería contener funcionalidad relacionada con los usuarios de marketing
    public class MarketingUsers
    {
    }
}
