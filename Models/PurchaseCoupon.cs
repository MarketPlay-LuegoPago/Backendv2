using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Models
{
    public class PurchaseCoupon
    {
      [Key]
      public int Id { get; set; }
      public int PurchaseId { get; set; }
      public int CouponId { get; set; }
      public Purchase? Purchase { get; set; }
      public Coupons? Coupon { get; set; }
    }
}