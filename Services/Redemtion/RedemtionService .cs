/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Data;

namespace Backengv2.Services.Redemtion
{
    public class RedemtionService 
    {
         private readonly IRedemtionRepository _redemtionRepository;

        private readonly BaseContext _context;

        public RedemtionService(IRedemtionRepository redemtionRepository)
        {
            _redemtionRepository = redemtionRepository ?? throw new ArgumentNullException(nameof(redemtionRepository));
        }

    public Result ValidateAndRedeemCoupon(string userId, int purchaseId, string couponCode)
    {
        var purchaseCoupon = GetPurchaseCouponByUserAndPurchaseId(userId, purchaseId,couponId);
        if (purchaseCoupon = null)
        {
            return new Result { Success = false, Message = "Relación entre la compra y el cupón no encontrada." };
        }

        var coupon = GetCouponById(purchaseCoupon.couponId);
        var purchase = GetPurchaseById(purchaseId);

        if (coupon == null || purchase == null)
        {
            return new Result { Success = false, Message = "Cupón o compra no válida." };
        }

        if (coupon.ValidUntil < DateTime.Now)
        {
            return new Result { Success = false, Message = "El cupón ha expirado." };
        }

        if (coupon.CurrentUses >= coupon.UsageLimit)
        {
            return new Result { Success = false, Message = "El cupón ha alcanzado su límite de usos." };
        }

        if (purchase.Price < coupon.PriceRangeMin || purchase.Price > coupon.PriceRangeMax)
        {
            return new Result { Success = false, Message = "El cupón no es aplicable a esta compra." };
        }

        _couponRepository.IncrementCouponUsage(purchaseCoupon.CouponId);

        return new Result { Success = true, Message = "Cupón redimido exitosamente.", DiscountApplied = coupon.Discount };
    }

    public PurchaseCoupon? GetPurchaseCouponByUserAndPurchaseId(string userId, int purchaseId)
    {
        return _couponRepository.GetPurchaseCouponByUserAndPurchaseId(userId, purchaseId);
    }

    public Coupon? GetCouponById(int couponId)
    {
        return _couponRepository.GetCouponById(couponId);
    }

    public Purchase? GetPurchaseById(int purchaseId)
    {
        return _couponRepository.GetPurchaseById(purchaseId);
    }
    }
} */