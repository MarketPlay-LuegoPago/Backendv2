using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Services.Coupons;
using Backengv2.Models;
using Backengv2.Data;
using Backengv2.Dtos;

namespace Backengv2.Services.Coupons
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetByIdAsync(int id);
        Task UpdateCouponAsync(Coupon coupon);
}}