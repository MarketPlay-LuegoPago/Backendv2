using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Models;

namespace Backengv2.Services.Redemtion
{
    public interface IRedemtionRepository
    {
        // Devuelve un objeto PurchaseCoupon que representa la relación entre un usuario y una compra específica, si existe.
        PurchaseCoupon? GetPurchaseCouponByUserAndPurchaseId(string userId, int purchaseId);

        // Devuelve un objeto Coupon por su ID.
        Coupon? GetCouponById(int couponId);

        // Devuelve un objeto Purchase por su ID.
        Purchase? GetPurchaseById(int purchaseId);

        // Incrementa el contador de uso de un cupón específico.
        void IncrementCouponUsage(int couponId);
    }
}
