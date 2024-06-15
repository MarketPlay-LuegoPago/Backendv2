using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Models;
using Backengv2.Services.Coupons;

namespace Backengv2.Controllers.Coupons
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponCreateController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponCreateController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CouponDetailDto>> CreateCoupon([FromBody] CouponCreateDto couponCreateDto)
        {
            try
            {
                var couponEntity = _mapper.Map<Coupon>(couponCreateDto);

                // Aquí podrías agregar validaciones adicionales si es necesario

                await _couponRepository.AddCouponAsync(couponEntity);

                var couponDetailDto = _mapper.Map<CouponDetailDto>(couponEntity);

                return CreatedAtAction(nameof(GetCouponById), new { id = couponDetailDto.id }, couponDetailDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Manejo básico de errores, puedes personalizar según necesites
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CouponDetailDto>> GetCouponById(int id)
        {
            try
            {
                var coupon = await _couponRepository.GetByIdAsync(id);

                if (coupon == null)
                {
                    return NotFound();
                }

                var couponDetailDto = _mapper.Map<CouponDetailDto>(coupon);
                return Ok(couponDetailDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Manejo básico de errores, puedes personalizar según necesites
            }
        }
    }
}
