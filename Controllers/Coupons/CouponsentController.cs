using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Data;
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Mvc;

namespace Backengv2.Controllers.Coupons // Define el espacio de nombres para el controlador
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    public class CouponsentController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        // Campo privado para el repositorio de cupones
        private readonly ICouponRepository _couponRepository;

        // Constructor que inyecta el repositorio de cupones
        public CouponsentController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        // Acción HTTP POST para enviar un cupón a los clientes
        [HttpPost("send/{couponId}")]
        public async Task<IActionResult> SendCouponToCustomers(int couponId, [FromBody] string message, [FromQuery] List<int> marketplaceUserIds)
        {
            try
            {
                // Llama al método del repositorio para enviar el cupón a los clientes
                var success = await _couponRepository.SendCouponToCustomersAsync(couponId, message, marketplaceUserIds);
                if (success)
                {
                    // Devuelve una respuesta HTTP 200 OK si el cupón se envía exitosamente
                    return Ok("Cupón enviado a todos los clientes exitosamente.");
                }

                // Devuelve una respuesta HTTP 400 Bad Request si hay un problema al enviar los cupones
                return BadRequest("Hubo un problema enviando los cupones.");
            }
            catch (Exception ex)
            {
                // Devuelve una respuesta HTTP 500 Internal Server Error si ocurre una excepción
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
