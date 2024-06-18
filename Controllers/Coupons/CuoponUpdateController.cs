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
    [ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CouponUpdateController : ControllerBase
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;

    public CouponUpdateController(ICouponRepository couponRepository, IMapper mapper)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCoupon(int id, [FromBody] CuponUpdateDto cuponUpdateDto)
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

       if (coupon.MarketingUserId != userId)
        {
            return Forbid("No tienes permiso para editar este cupón.");
        } 

        if (coupon.status == "redimido")
        {
            return BadRequest("El cupón no se puede editar porque ya ha sido utilizado.");
        }

        // Mapea los datos del objeto CouponDto al objeto Coupon
        var couponEntity = _mapper.Map<Coupon>(cuponUpdateDto);
        couponEntity.id = id; // Asegurar que el ID del cupón no cambie

        try
        {
            await _couponRepository.UpdateCouponAsync(couponEntity);

            return Ok("Cupón actualizado correctamente.");
        }
        catch (Exception ex)
        {
            // Delegar el manejo de excepciones no controladas al Middleware de Manejo de Errores
            throw; // Aquí el middleware capturará y manejará el error
        }
    }
}

}

    

 