using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Backeng.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using backend.Dto;


namespace Authcontroller
{
    public class LoginController : ControllerBase
    {
        private readonly BaseContext _context;
        public LoginController(BaseContext context)
        {
            _context = context;
        }
        [HttpPost("Login")]
         public async Task <IActionResult>Login([FromBody] Login login) 
         {
            var MarketingUser = await _context.MarketingUser.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);
            if( MarketingUser == null)
            {
                return BadRequest("The Query is invalid or the user does´nt exist");
            }
            else

            {
                var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FTGUNMIMGI4MFI4J2RÑNUFRFFM4FN4874H4BBFHRF"));
                var signinCredentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
                //Agregamos para recibir el id de la persona que inicia la sesión
                var Claims = new List<Claim>
                {
                    //new Claim(ClaimTypes.NameIdentifier.MarketingUser.id.toString())  
                };
                var TokenConfigure = new JwtSecurityToken(
                    issuer: "",
                    audience: "",   //Aqui van los endPoints
                    claims : new List<Claim>(),
                    expires : DateTime.Now.AddHours(1), 
                    signingCredentials : signinCredentials
                );
                var token = new JwtSecurityTokenHandler().WriteToken(TokenConfigure);

                return Ok(new {token});
            }
         }
    } 
}


