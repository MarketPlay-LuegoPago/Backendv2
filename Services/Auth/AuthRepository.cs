using Backeng.Data;
using Backengv2.Models;
using Backend.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens; //Jwt
using System.Text;
using System.Security.Claims; // DataNotation of Jwt
using BCrypt.Net;

public class AuthRepository : IAuthRepository
{
    private readonly BaseContext _context;
    public AuthRepository(BaseContext context)
    {
        _context = context;
    }
    public string GenerateToken(MarketingUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity (new  Claim[]{
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token= tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    //Metodo para encriptar la contraseña 
    // public bool VeriryPassword(string Password, string HashedPassword)
    // {
    //     return BCrypt.Net.BCrypt.Verify(Password, HashedPassword);
    // }
}
