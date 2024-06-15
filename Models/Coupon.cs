using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? Expiration_date { get; set; }
        public string? DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public string? UseType { get; set; }
        public int Quantity_uses { get; set; }
        public decimal MinPurchaseAmount { get; set; }
        public decimal MaxPurchaseAmount { get; set; }
        public string? Status { get; set; }
        public int RedemptionLimit { get; set; }
        public int CurrentRedemptions { get; set; }
        public int MarketingUserId { get; set; }
        public MarketingUser? MarketingUser { get; set; }
    }
}