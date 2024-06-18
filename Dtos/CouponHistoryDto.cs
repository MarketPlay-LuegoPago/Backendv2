using System;

namespace Backengv2.Dtos
{
    public class CouponHistoryDto
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string FieldChanged { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int ChangedByUser { get; set; }

      public string CouponName { get; set; }
      public string MarketingUsername { get; set; }
    }
}
