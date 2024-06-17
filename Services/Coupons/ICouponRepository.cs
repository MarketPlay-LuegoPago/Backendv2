using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Services.Coupons;
using Backengv2.Models;
using Backengv2.Data;
using Backengv2.Dtos;

namespace Backengv2.Services.Coupons
{
    public interface ICouponRepository
    {

        Task<Coupon> GetByIdAsync(int id);
        Task UpdateCouponAsync(Coupon couponEntity);
        
        Task<bool> SendCouponToCustomersAsync(int couponId, string message, List<int> customerIds);
        Task<IEnumerable<Coupon>> GetAllCouponsAsync();
        Task<IEnumerable<Coupon>> GetCouponsByDateRangeAsync(DateTime? StartDate, DateTime? endDate);
        Task<IEnumerable<Coupon>> GetCouponsByCreatorNameAsync(string creatorName); 
        Task<IEnumerable<Coupon>> GetCouponsByActivationDateAsync(DateTime ActivationDate);
        Task<IEnumerable<Coupon>> GetCouponsByExpirationDateAsync(DateTime ExpirationDate);
        Task<IEnumerable<Coupon>> GetCouponsActiveAsync();

        Task AddCouponAsync(Coupon coupon);
    }
}
