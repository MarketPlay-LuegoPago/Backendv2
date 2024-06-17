using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backengv2.Models;

namespace Backengv2.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
          
        }
        public DbSet<MarketplaceUser> MarketplaceUser { get; set; }
        public DbSet<MarketingUser> MarketingUsers { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponHistory> CouponHistories { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<CouponUsage> CouponUsages { get; set; }
        public DbSet<PurchaseCoupon> PurchaseCoupons { get; set; }
        public DbSet<CouponsSent> CouponsSent { get; set; }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>()
                .HasMany(c => c.CouponHistories)
                .WithOne(ch => ch.Coupon)
                .HasForeignKey(ch => ch.CouponId);
        }
    }

    
}