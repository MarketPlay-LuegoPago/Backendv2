using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Models
{
    public class Purchase
    {
        [Key]
      public int Id { get; set; }
      public int UserId { get; set; }
      public DateTime Date { get; set; }
      public decimal amount { get; set; }
      public MarketplaceUser? User { get; set; }
    }
}