using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Services.Coupons;
using Backengv2.Models;
using Backengv2.Data;

namespace Backengv2.Services.Coupons
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllCouponsAsync();
        Task<IEnumerable<Coupon>> GetCouponsByDateRangeAsync(DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<Coupon>> GetCouponsByCreatorNameAsync(string creatorName); 
        Task<IEnumerable<Coupon>> GetCouponsByActivationDateAsync(DateTime activationDate);
        Task<IEnumerable<Coupon>> GetCouponsByExpirationDateAsync(DateTime expirationDate);
        Task<IEnumerable<Coupon>> GetCouponsActiveAsync();
    }
}