using System.Security.Claims;
using Backengv2.Data;
using Backengv2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Backengv2.Services.Coupons;

namespace Backengv2.Controllers.Coupons
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Requiere autorización con esquema de autenticación JWT Bearer
    public class StatusCouponController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        private readonly BaseContext _context; // Contexto de la base de datos
        private readonly ICouponRepository _couponRepository; // Repositorio de cupones

        // Constructor que inyecta el contexto de la base de datos y el repositorio de cupones
        public StatusCouponController(BaseContext context, ICouponRepository couponRepository)
        {
            _context = context;
            _couponRepository = couponRepository;
        }

        // Acción HTTP PUT para actualizar el estado de un cupón por su ID
        [HttpPut]
        public async Task<IActionResult> UpdateCouponStatus(int id)
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Obtiene el ID de usuario del token JWT
            if (userIdClaim == null)
            {
                return Unauthorized("Usuario no autenticado."); // Devuelve un error si el usuario no está autenticado
            }

            var userId = int.Parse(userIdClaim); // Convierte el ID de usuario a entero

            var coupon = await _couponRepository.GetByIdAsync(id); // Busca el cupón por su ID
            if (coupon == null)
            {
                return NotFound("Cupón no encontrado."); // Devuelve un error 404 si el cupón no se encuentra
            }

            if (coupon.MarketingUserid != userId)
            {
                return Forbid("No tienes permiso para cambiar el estado de este cupón."); // Devuelve un error si el usuario no tiene permiso para cambiar el estado del cupón
            }

            // Cambia el estado del cupón de activo a inactivo y viceversa
            if (coupon.status == "inactive")
            {
                coupon.status = "active";
            }
            else if (coupon.status == "active")
            {
                coupon.status = "inactive";
            }
            else
            {
                return BadRequest("Cupón no existe"); // Devuelve un error si el estado del cupón no es válido
            }

            await _couponRepository.UpdateCouponAsync(coupon); // Actualiza el cupón en la base de datos

            return Ok($"Cupón cambiado a {coupon.status} correctamente."); // Devuelve un mensaje de éxito con el nuevo estado del cupón
        }
    }
}
