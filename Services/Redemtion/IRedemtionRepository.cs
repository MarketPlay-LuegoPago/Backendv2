using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Models;

namespace Backengv2.Services.Redemtion
{
    public interface IRedemtionRepository
    {

        PurchaseCoupon? GetPurchaseCouponByUserAndPurchaseId(string userId, int purchaseId);
        Coupon? GetCouponById(int couponId);
        Purchase? GetPurchaseById(int purchaseId);
        void IncrementCouponUsage(int couponId);
    
        
    }
}