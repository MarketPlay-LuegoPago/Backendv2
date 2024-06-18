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
using System.IdentityModel.Tokens.Jwt;




namespace Backengv2.Controllers.Login
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

         private readonly BaseContext _context;
      public AuthController(BaseContext context)
      {
          _context = context;
            }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] MarketingUser employee)
        {
            var MarketingUser = await _context.MarketingUsers
                .FirstOrDefaultAsync(e => e.Email == employee.Email && e.Password == employee.Password);
            
            if (MarketingUser == null)
            {
                return BadRequest("Error en Correo o Contraseña");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ncjdncjvurbuedxwn233nnedxee+dfr-"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, MarketingUser.id.ToString())
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5205",
                audience: "https://localhost:5205",
                claims: claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { Token = tokenString });
        }
    }



}
