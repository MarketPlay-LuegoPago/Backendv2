using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Dtos
{
    public class CouponHistoryDto
    {
         public int id { get; set; }
        public int CouponId { get; set; }
        public DateTime change_date { get; set; }
        public string? FieldChanged { get; set; }
        public string ?OldValue { get; set; }
        public string? NewValue { get; set; }
         public int ChangedByUser { get; set; }
    }
}