using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Dtos
{
    public class CouponDetailDto
    {
         public int id { get; set; }
          public string? name { get; set; }
          public string? description { get; set; }
          public DateTime activation_date { get; set; }
          public DateTime expiration_date { get; set; }
          public decimal discount_value { get; set; }
          public int current_redemptions { get; set; }
          public string? MarketingUsername { get; set; }
        
    }
}