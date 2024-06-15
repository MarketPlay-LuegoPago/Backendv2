using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Services.Coupons;
using System;

namespace Backengv2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponFilterController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponFilterController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        [HttpGet("byDateRange")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsByDateRange(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var coupons = await _couponRepository.GetCouponsByDateRangeAsync(startDate, endDate);
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        [HttpGet("byCreatorName")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsByCreatorName([FromQuery] string creatorName)
        {
            var coupons = await _couponRepository.GetCouponsByCreatorNameAsync(creatorName);
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        [HttpGet("byActivationDate")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsByActivationDate([FromQuery] DateTime activationDate)
        {
            var coupons = await _couponRepository.GetCouponsByActivationDateAsync(activationDate);
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        [HttpGet("byExpirationDate")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsByExpirationDate([FromQuery] DateTime expirationDate)
        {
            var coupons = await _couponRepository.GetCouponsByExpirationDateAsync(expirationDate);
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        
        [HttpGet("byCouponActive")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsActive()
        {
            var coupons = await _couponRepository.GetCouponsActiveAsync();
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }
    }
}
