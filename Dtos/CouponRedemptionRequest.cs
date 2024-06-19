using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Dtos
{
    public class CouponRedemptionRequest
    {
        public int userId { get; set; }
        public int PurchaseId { get; set; }
        public int couponId { get; set; }
        public decimal transaction_amount { get; set; }
    }
}