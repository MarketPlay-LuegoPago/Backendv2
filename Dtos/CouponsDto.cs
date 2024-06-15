using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Dtos
{
    public class CouponsDto
    {
         public int id { get; set; }
          public string? name { get; set; }
          public string? description { get; set; }
          public DateTime creation_date { get; set; }
          public DateTime activation_date { get; set; }
          public DateTime expiration_date { get; set; }
          public string? discount_type { get; set; }
          public decimal discount_value { get; set; }
          public string? use_type { get; set; }
          public int quantity_uses { get; set; }
          public decimal min_purchase_amount { get; set; }
          public decimal max_purchase_amount { get; set; }
          public string? status { get; set; }
          public int redemption_limit { get; set; }
          public int current_redemptions { get; set; }
         public string? MarketingUsername { get; set; }
    }
}