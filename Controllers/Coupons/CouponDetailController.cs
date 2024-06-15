using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backengv2.Services.Coupons;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Data; 
using Microsoft.EntityFrameworkCore;
using Backengv2.Models; 

namespace Backengv2.Controllers.Coupons
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
    private readonly BaseContext _context;
    private readonly IMapper _mapper;

    public CouponController(BaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CouponDetailDto>> GetCouponById(int id)
    {
        var coupon = await _context.Coupons.FindAsync(id);

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