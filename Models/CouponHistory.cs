using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Models
{
    public class CouponHistory
    {
    [Key]
    public int id { get; set; }
    public int CouponId { get; set; }
    public DateTime ChangeDate { get; set; }
    public string? FieldChanged { get; set; }
    public decimal OldValue { get; set; }
    public string? NewValue { get; set; }
    
    [ForeignKey("CouponId")]
    public Coupon? Coupon { get; set; }
    }
}