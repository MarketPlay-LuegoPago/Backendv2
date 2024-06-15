using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Dtos
{
    public class CouponsDto
    {
         public int id { get; set; }
          public string? Name { get; set; }
          public string? Description { get; set; }
          public DateTime Creation_date { get; set; }
          public DateTime Activation_date { get; set; }
          public DateTime Expiration_date { get; set; }
          public string? DiscountType { get; set; }
          public decimal Discount_value { get; set; }
          public string? UseType { get; set; }
          public int Quantity_uses { get; set; }
          public decimal Min_purchase_amount { get; set; }
          public decimal Max_purchase_amount { get; set; }
          public string? Status { get; set; }
          public int Redemption_limit { get; set; }
          public int Current_redemptions { get; set; }
         public string? MarketingUsername { get; set; }
    }
}