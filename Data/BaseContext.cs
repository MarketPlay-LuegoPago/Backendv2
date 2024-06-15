using Backengv2.Models;
using Microsoft.EntityFrameworkCore;

namespace Backeng.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {

        }
        public DbSet <MarketingUser> MarketingUser {get; set; }
    }
}