using Backengv2.Models;
using BCrypt.Net;


namespace Backend.Services
{
    public interface IAuthRepository
    {
        bool VeriryPassword(string Password, string HashedPassword); //Metodo para Hashear la contraseña (Encriptar)
        string GenerateToken(MarketingUser user); //Generamos el token que llamaremos en el controlador
        
    }
}