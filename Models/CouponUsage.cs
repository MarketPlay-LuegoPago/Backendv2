using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Models
{
    public class CouponUsage
    {
      [Key]
      public int CouponId { get; set; }
      public int UserId { get; set; }
      public DateTime UsageDate { get; set; }
      public decimal TransactionAmount { get; set; }
      public string? Status { get; set; }
      public Coupons? Coupon { get; set; }
      public MarketplaceUser? User { get; set; }
    }
}