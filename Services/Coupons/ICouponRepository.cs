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
        // Métodos para obtener cupones
        Task<Coupon> GetByIdAsync(int id);
        Task<IEnumerable<Coupon>> GetAllCouponsAsync();
        Task<IEnumerable<Coupon>> GetCouponsByDateRangeAsync(DateTime? StartDate, DateTime? endDate);
        Task<IEnumerable<Coupon>> GetCouponsByCreatorNameAsync(string creatorName);
        Task<IEnumerable<Coupon>> GetCouponsByActivationDateAsync(DateTime ActivationDate);
        Task<IEnumerable<Coupon>> GetCouponsByExpirationDateAsync(DateTime ExpirationDate);
        Task<IEnumerable<Coupon>> GetCouponsActiveAsync();

        // Método para agregar un nuevo cupón
        Task AddCouponAsync(Coupon coupon);

        // Métodos para gestionar el historial de cupones
        Task<IEnumerable<CouponHistoryDto>> GetAllCouponHistoriesAsync();

        // Métodos para actualizar y eliminar cupones
        Task UpdateCouponAsync(Coupon couponEntity);
        Task DeleteCouponAsync(Coupon coupon);

        // Método para cambiar el estado de un cupón
        Task statuschangeCouponAsync(Coupon coupon);

        // Métodos para verificar el uso de cupones y agregar uso de cupones
        Task<bool> IsCouponRedeemedAsync(int userId, int couponId);
        Task AddCouponUsageAsync(CouponUsage usage);

        // Método para obtener cupones por ID de usuario de marketing
        Task<List<CouponsDto>> GetCouponsByMarketingUserIdAsync(int marketingUserId);
    }
}
