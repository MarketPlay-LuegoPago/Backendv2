using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Backengv2.Data;
using Backengv2.Dtos;
using Backengv2.Models;
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backengv2.Controllers.Coupons
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Requiere autorización con esquema de autenticación JWT Bearer
    public class CouponUpdateController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        private readonly ICouponRepository _couponRepository; // Campo privado para el repositorio de cupones
        private readonly IMapper _mapper; // Campo privado para el mapeador de objetos

        // Constructor que inyecta el repositorio de cupones y el mapeador de objetos
        public CouponUpdateController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        // Acción HTTP PUT para actualizar un cupón por su ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, [FromBody] CuponUpdateDto cuponUpdateDto)
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
                return Forbid("No tienes permiso para editar este cupón."); // Devuelve un error si el usuario no tiene permiso para editar el cupón
            }

            if (coupon.status == "redimido")
            {
                return BadRequest("El cupón no se puede editar porque ya ha sido utilizado."); // Devuelve un error si el cupón ya ha sido utilizado
            }

            // Mapea los datos del objeto CuponUpdateDto al objeto Coupon
            var couponEntity = _mapper.Map<Coupon>(cuponUpdateDto);
            couponEntity.id = id; // Asegura que el ID del cupón no cambie

            try
            {
                await _couponRepository.UpdateCouponAsync(couponEntity); // Llama al método del repositorio para actualizar el cupón

                return Ok("Cupón actualizado correctamente."); // Devuelve una respuesta HTTP 200 OK si el cupón se actualiza correctamente
            }
            catch (Exception ex)
            {
                // Delega el manejo de excepciones no controladas al Middleware de Manejo de Errores
                throw; // El middleware capturará y manejará el error
            }
        }
    }
}
