using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Models;
using Microsoft.EntityFrameworkCore;
using Backengv2.Services.Coupons;
using Backengv2.Data;

namespace Backengv2.Services.Coupons
{
    public class CouponRepository : ICouponRepository
    {
         private readonly BaseContext _context;

        public CouponRepository(BaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            return await _context.Coupons.ToListAsync();
        }
    }
}