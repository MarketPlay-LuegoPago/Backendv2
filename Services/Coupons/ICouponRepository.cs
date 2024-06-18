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

        Task<IEnumerable<CouponHistoryDto>> GetAllCouponHistoriesAsync();
       
        Task<IEnumerable<CouponsDto>> GetCouponsForUserAsync(int userId, bool isAdmin);
  

        Task<Coupon> GetById(int id);
        Task DeleteCouponAsync(Coupon coupon);

      //  Task<Coupon?> GetByIdAsync(int id);
        Task statuschangeCouponAsync(Coupon coupon);
            Task<bool> IsCouponRedeemedAsync(int userId, int couponId);
    Task AddCouponUsageAsync(CouponUsage usage);
    Task<Coupon> GetCouponByIdAsync(int couponId);

    }
}
