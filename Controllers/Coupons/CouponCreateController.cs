using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Models;
using Backengv2.Services.Coupons;
using Backengv2.Data;
using Microsoft.EntityFrameworkCore;

namespace Backengv2.Controllers.Coupons
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponCreateController : ControllerBase
    {
        private readonly BaseContext _context;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponCreateController(BaseContext context, ICouponRepository couponRepository, IMapper mapper)
        { 
            _context = context;
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

      [HttpPost]
      public async Task<ActionResult<CouponsDto>> CreateCoupon([FromBody] CouponCreateDto couponCreateDto)
      {
          try
          {
              var marketingUser = await _context.MarketingUsers.FirstOrDefaultAsync(u => u.Id == couponCreateDto.MarketingUserId);
              if (marketingUser == null)
              {
                  return BadRequest("El ID de usuario de marketing proporcionado no es válido.");
              }

              var couponEntity = _mapper.Map<Coupon>(couponCreateDto);
              
              // Asignar las fechas correctamente
              couponEntity.CreationDate = DateTime.UtcNow;
              couponEntity.ActivationDate = couponCreateDto.ActivationDate;
              couponEntity.expiration_date = couponCreateDto.expiration_date;

              await _couponRepository.AddCouponAsync(couponEntity);

              var createdCouponDto = _mapper.Map<CouponsDto>(couponEntity);

              return CreatedAtAction(nameof(GetCouponById), new { id = createdCouponDto.id }, createdCouponDto);
          }
          catch (Exception ex)
          {
              // Capturar el error exacto con más detalles
              var errorMessage = $"Error interno del servidor: {ex.Message}";
              if (ex.InnerException != null)
              {
                  errorMessage += $" Inner Exception: {ex.InnerException.Message}";
              }
              errorMessage += $" Stack Trace: {ex.StackTrace}";

              return StatusCode(500, errorMessage);
          }
      }





      [HttpGet("{id}")]
      public async Task<ActionResult<CouponsDto>> GetCouponById(int id)
      {
          var coupon = await _couponRepository.GetByIdAsync(id);

          if (coupon == null)
          {
              return NotFound();
          }

          var couponDto = _mapper.Map<CouponsDto>(coupon);
          return Ok(couponDto);
      }
    }
}
