using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Data;
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Mvc;

namespace Backengv2.Controllers.Coupons
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsentController : ControllerBase
    {
    private readonly ICouponRepository _couponRepository;

    public CouponsentController(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    [HttpPost("send/{couponId}")]
    public async Task<IActionResult> SendCouponToCustomers(int couponId, [FromBody] string message, [FromQuery] List<int> marketplaceUserIds)
    {
        try
        {
            var success = await _couponRepository.SendCouponToCustomersAsync(couponId, message, marketplaceUserIds);
            if (success)
            {
                return Ok("Cupón enviado a todos los clientes exitosamente.");
            }

            return BadRequest("Hubo un problema enviando los cupones.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }
    }
}