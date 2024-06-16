using Microsoft.AspNetCore.Mvc;
using Backeng.Data;
using Microsoft.EntityFrameworkCore;
using backend.Dto;
using Backend.Services;



namespace Authcontroller
{
    public class LoginController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly BaseContext _context;
        public LoginController(IAuthRepository authRepository ,BaseContext context)
        {
            _context = context;
            _authRepository = authRepository;
        }
        [HttpPost("Login")]
         public async Task <IActionResult>Login([FromBody] Login login) 
         {
            var user = await _context.MarketingUser.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);
            if( user == null)
            {
                return Unauthorized("The Query is invalid or the user doesn´t exist");
            }
            var token = _authRepository.GenerateToken(user);
            return Ok(new {token});
         }
    } 
}


