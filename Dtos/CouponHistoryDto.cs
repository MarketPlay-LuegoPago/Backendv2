using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Models;

namespace Backengv2.Dtos
{
    public class CouponHistoryDto
    {
         public int Id { get; set; }
        public int CouponId { get; set; }
         [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }
        public DateTime change_date { get; set; }
        public string? FieldChanged { get; set; }
        public string ?OldValue { get; set; }
        public string? NewValue { get; set; }
         public int ChangedByUser { get; set; }
        [ForeignKey("ChangedByUser")]
        public MarketingUser? MarketingUser { get; set; }
    }
}