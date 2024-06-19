using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Backengv2.Data;
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backengv2.Controllers.Coupons // Define el espacio de nombres para el controlador
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Requiere autorización con esquema de autenticación JWT Bearer
    public class CouponDeleteController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        // Campos privados para el contexto de la base de datos, el repositorio de cupones y el mapeador de objetos
        private readonly BaseContext _context;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        // Constructor que inyecta el contexto de la base de datos, el repositorio de cupones y el mapeador de objetos
        public CouponDeleteController(BaseContext context, ICouponRepository couponRepository, IMapper mapper)
        {
            _context = context;
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        // Acción HTTP PUT para eliminar un cupón por su ID
        [HttpPut]
        //[Route("/delete/{id}")] // Define la ruta específica para eliminar un cupón
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            // Obtiene el ID del usuario autenticado desde el token JWT
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                // Devuelve un error si el usuario no está autenticado
                return Unauthorized("Usuario no autenticado.");
            }

            // Convierte el ID del usuario a un entero
            var userId = int.Parse(userIdClaim);

            // Busca el cupón por su ID en el repositorio de cupones
            var coupon = await _couponRepository.GetById(id);
            if (coupon == null)
            {
                // Devuelve un error 404 si el cupón no se encuentra
                return NotFound("Cupón no encontrado.");
            }

            // Verifica si el cupón pertenece al usuario autenticado
            if (coupon.MarketingUserid != userId)
            {
                // Devuelve un error si el usuario no tiene permiso para eliminar el cupón
                return Forbid("No tienes permiso para eliminar este cupón.");
            }

            // Verifica si el cupón ya ha sido utilizado
            if (coupon.status == "redimido")
            {
                // Devuelve un error si el cupón ya ha sido utilizado
                return BadRequest("El cupón no se puede eliminar porque ya ha sido utilizado.");
            }

            // Cambia el estado del cupón a "deleted"
            coupon.status = "deleted";

            // Elimina el cupón del repositorio de cupones
            await _couponRepository.DeleteCouponAsync(coupon);

            // Devuelve una respuesta HTTP 200 OK indicando que el cupón ha sido eliminado
            return Ok("Cupón eliminado correctamente.");
        }
    }
}
