using Backeng.Data;
using Backengv2.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using BCrypt.Net;

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
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
