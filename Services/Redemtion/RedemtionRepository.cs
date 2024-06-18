/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Data;
using Backengv2.Models;
using Backengv2.Services.Coupons;

namespace Backengv2.Services.Redemtion
{
    public class RedemtionRepository:IRedemtionRepository
    {

            
        private readonly BaseContext _context;
        private readonly ICouponRepository  _CouponRepository;
  
        private readonly List<Coupon> _coupons = new List<Coupon>();
        private readonly List<Purchase> _purchases = new List<Purchase>();
        private readonly List<PurchaseCoupon> _purchaseCoupons = new List<PurchaseCoupon>();
        

        public RedemtionRepository(BaseContext context)
        {
            _context = context;
        
        }


            // Representaciones ficticias de colecciones para la relación entre la compra y el cupón, así como de los modelos

            public PurchaseCoupon? GetPurchaseCouponByUserAndPurchaseId(string userId, int purchaseId)
            {
                // Encuentra la relación entre la compra y el cupón basado en el usuario y el ID de compra
                return _purchaseCoupons.FirstOrDefault(pc => pc.Purchase.UserId == userId && pc.PurchaseId == purchaseId);
            }

            public Coupon? GetCouponById(int couponId)
            {
                // Encuentra el cupón basado en su ID
                return _coupons.FirstOrDefault(c => c.id == couponId);
            }

            public Purchase? GetPurchaseById(int purchaseId)
            {
                // Encuentra la compra basado en su ID
                return _purchases.FirstOrDefault(p => p.Id == purchaseId);
            }

            public void IncrementCouponUsage(int couponId)
            {
                // Incrementa el uso del cupón cuando se aplica la redención
                var coupon = _coupons.FirstOrDefault(c => c.id == couponId);
                if (coupon != null)
                {
                    coupon.CurrentRedemptions++;
                }
            }

        
    }
} */