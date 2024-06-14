using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backengv2.Models
{
    public class MarketplaceUser
    {
        [Key]
        public int Id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}