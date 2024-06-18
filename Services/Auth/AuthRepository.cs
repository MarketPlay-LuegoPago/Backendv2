using Backengv2.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using BCrypt.Net;
using Backengv2.Data;

namespace Backend.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BaseContext _context;
        private readonly string _jwtSecret;
        public AuthRepository(BaseContext context, string jwtSecret)
        {
            _context = context;
            _jwtSecret = jwtSecret;
        }
        public bool VeriryPassword(string Password, string HashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(Password, HashedPassword);
        }
       public string GenerateToken(MarketingUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenOptions = new JwtSecurityToken(
                issuer : @Environment.GetEnvironmentVariable("jwtUrl"),
                audience : @Environment.GetEnvironmentVariable("jwtUrl"),
                claims : new List<Claim>(),
                expires : DateTime.Now.AddHours(1), 
                signingCredentials : new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            // var token = new tokenHandler()(tokenOptions);
            return tokenHandler.WriteToken(tokenOptions);
        }

        // object IAuthRepository.GenerateToken(MarketingUser user)
        // {
        //     throw new NotImplementedException();
        // }
    }

    public class MarketingUsers   //Clases para Generar Tokens
    {
    }
}
