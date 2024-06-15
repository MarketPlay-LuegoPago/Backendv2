using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Backengv2.Dtos
{
    public class CouponCreateDto
    {
        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime ActivationDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public string? DiscountType { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public string? UseType { get; set; }

        [Required]
        public int QuantityUses { get; set; }

        [Required]
        public decimal MinPurchaseAmount { get; set; }

        [Required]
        public decimal MaxPurchaseAmount { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public int RedemptionLimit { get; set; }

        [Required]
        public int MarketingUserId { get; set; }
    }
}