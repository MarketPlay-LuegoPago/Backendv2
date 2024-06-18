using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Models;
using Backengv2.Services.Coupons;
using Backengv2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backengv2.Controllers.Coupons
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponCreateController : ControllerBase
    {
        private readonly BaseContext _context;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponCreateController> _logger;

        public CouponCreateController(BaseContext context, ICouponRepository couponRepository, IMapper mapper, ILogger<CouponCreateController> logger)
        { 
            _context = context;
            _couponRepository = couponRepository;
            _mapper = mapper;
            _logger = logger;
        }

[HttpPost]
public async Task<ActionResult<CouponsDto>> CreateCoupon([FromBody] CouponCreateDto couponCreateDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState); // Devuelve los errores de validación del modelo
    }

    var marketingUser = await _context.MarketingUsers.FirstOrDefaultAsync(u => u.id == couponCreateDto.MarketingUserid);
    if (marketingUser == null)
    {
        return BadRequest("El ID de usuario de marketing proporcionado no es válido.");
    }

    var couponEntity = _mapper.Map<Coupon>(couponCreateDto);
    couponEntity.CreationDate = DateTime.UtcNow;
    couponEntity.MarketingUser = marketingUser;

    await _couponRepository.AddCouponAsync(couponEntity);

    var createdCoupon = await _context.Coupons.Include(c => c.MarketingUser).FirstOrDefaultAsync(c => c.id == couponEntity.id);
    var createdCouponDto = _mapper.Map<CouponsDto>(createdCoupon);

    return CreatedAtAction(nameof(GetCouponById), new { id = createdCouponDto.Id }, createdCouponDto);
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
