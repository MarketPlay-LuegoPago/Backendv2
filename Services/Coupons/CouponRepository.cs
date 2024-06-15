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
           return await _context.Coupons.Include(c => c.MarketingUser).ToListAsync();
        }

         public async Task<IEnumerable<Coupon>> GetCouponsByDateRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Coupon> query = _context.Coupons.Include(c => c.MarketingUser);

            if (startDate.HasValue)
            {
                query = query.Where(c => c.activation_date >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.expiration_date <= endDate);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Coupon>> GetCouponsByCreatorNameAsync(string creatorName)
        {
            return await _context.Coupons.Include(c => c.MarketingUser)
                                         .Where(c => c.MarketingUser.Username == creatorName)
                                         .ToListAsync();
        }

        public async Task<IEnumerable<Coupon>> GetCouponsByActivationDateAsync(DateTime activationDate)
        {
            return await _context.Coupons.Include(c => c.MarketingUser)
                                         .Where(c => c.activation_date == activationDate.Date)
                                         .ToListAsync();
        }

        public async Task<IEnumerable<Coupon>> GetCouponsByExpirationDateAsync(DateTime expirationDate)
        {
            return await _context.Coupons.Include(c => c.MarketingUser)
                                         .Where(c => c.expiration_date == expirationDate.Date)
                                         .ToListAsync();
        }

         public async Task<IEnumerable<Coupon>> GetCouponsActiveAsync()
        {
            return await _context.Coupons.Include(c => c.MarketingUser)
                                         .Where(c => c.status == "active")
                                         .ToListAsync();
        }

    }
}