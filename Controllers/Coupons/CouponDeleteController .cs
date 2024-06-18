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

namespace Backengv2.Controllers.Coupons
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CouponDeleteController  : ControllerBase
    {
        private readonly BaseContext _context;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponDeleteController(BaseContext context, ICouponRepository couponRepository, IMapper mapper)
        { 
            _context = context;
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        [HttpPut]
        [Route("/delete/{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("Usuario no autenticado.");
            } 

          var userId = int.Parse(userIdClaim); 

            var coupon = await _couponRepository.GetById(id);
            if (coupon == null)
            {
                return NotFound("Cupón no encontrado.");
            }

             if (coupon.MarketingUserid != userId)
            {
                return Forbid("No tienes permiso para eliminar este cupón.");
            }
 
            if (coupon.status == "redimido")
            {
                return BadRequest("El cupón no se puede eliminar porque ya ha sido utilizado.");
            }

            coupon.status = "deleted";

            await _couponRepository.DeleteCouponAsync(coupon);

            return Ok("Cupón eliminado correctamente.");
        }
    }
}