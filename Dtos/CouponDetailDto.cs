using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Dtos
{
    public class CouponDetailDto
    {
         public int Id { get; set; }
          public string? Name { get; set; }
          public string? Description { get; set; }
          public DateTime Activation_date { get; set; }
          public DateTime expiration_date { get; set; }
          public decimal Discount_value { get; set; }
          public int Current_redemptions { get; set; }
          public string? MarketingUsername { get; set; }
        
    }
}