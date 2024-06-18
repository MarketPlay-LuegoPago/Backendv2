using Backengv2.Models; // Importación del espacio de nombres que contiene el modelo MarketingUser
using Backengv2.Data; // Importación del espacio de nombres que contiene BaseContext
using BCrypt.Net; // Importación del espacio de nombres que contiene BCrypt

namespace Backend.Services
{
    // Interfaz que define métodos para autenticación y generación de tokens JWT
    public interface IAuthRepository
    {
        // Método para verificar si la contraseña coincide con el hash almacenado
        bool VeriryPassword(string Password, string HashedPassword);

        // Método para generar un token JWT para el usuario proporcionado
        string GenerateToken(MarketingUser user);
    }
}
