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
     
      
      public int PurchaseId { get; set; }
      public DateTime usage_date { get; set; }
      public decimal transaction_amount { get; set; }
      public string? Status { get; set; }
       public int CouponId { get; set; }
      public Coupon? Coupon { get; set; }
      public int userId { get; set; }
      public MarketplaceUser? User { get; set; }
    }
}