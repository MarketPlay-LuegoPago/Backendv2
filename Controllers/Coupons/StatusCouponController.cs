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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StatusCouponController : ControllerBase
    {
        private readonly BaseContext _context;
        private readonly ICouponRepository _couponRepository;

        public StatusCouponController(BaseContext context, ICouponRepository couponRepository)
        {
            _context = context;
            _couponRepository = couponRepository;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCouponStatus(int id)
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("Usuario no autenticado.");
            }

            var userId = int.Parse(userIdClaim);

            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
            {
                return NotFound("Cupón no encontrado.");
            }

            // Verifica si el usuario autenticado es el dueño del cupón o tiene los permisos necesarios
            if (coupon.MarketingUserId != userId)
            {
                return Forbid("No tienes permiso para cambiar el estado de este cupón.");
            }

            // Cambia el estado del cupón y lo actualiza en la base de datos
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
                return BadRequest("Cupon no existe");
            }

            await _couponRepository.UpdateCouponAsync(coupon);

            return Ok($"Cupón cambiado a {coupon.status} correctamente.");
        }
    }
}
