using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Data;
using Microsoft.EntityFrameworkCore;
using Backengv2.Models;

namespace Backengv2.Controllers.Coupons
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponDetailController : ControllerBase
    {
        private readonly BaseContext _context;
        private readonly IMapper _mapper;

        public CouponDetailController(BaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CouponDetailDto>> GetCouponById(int id)
        {
            var coupon = await _context.Coupons
                .Include(c => c.MarketingUser) // Include the MarketingUser
                .FirstOrDefaultAsync(c => c.id == id);

            if (coupon == null)
            {
                return NotFound();
            }

            var couponDto = _mapper.Map<CouponDetailDto>(coupon);
            return Ok(couponDto);
        }

        // Otros métodos de tu controlador
    }
}
