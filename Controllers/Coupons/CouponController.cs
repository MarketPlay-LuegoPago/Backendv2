using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backengv2.Services.Coupons;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Data; 
using Microsoft.EntityFrameworkCore;
using Backengv2.Models; 

namespace Backengv2.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly BaseContext _context;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponController(BaseContext context, ICouponRepository couponRepository, IMapper mapper)
        {
            _context = context;
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCoupons()
        {
            var coupons = await _context.Coupons.ToListAsync();
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }
    }
}
