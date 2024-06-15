using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Models
{
    public class CouponsSent
    {
      [Key]
      public int Id { get; set; }
      public int UserId { get; set; }
      public int CouponId { get; set; }
      public MarketplaceUser? User { get; set; }
      public Coupon? Coupon { get; set; }
    }
}