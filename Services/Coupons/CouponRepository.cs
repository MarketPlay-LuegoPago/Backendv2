using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Data;
using Backengv2.Models;
using Backengv2.Services.Coupons;
using Microsoft.EntityFrameworkCore;

public class CouponRepository : ICouponRepository
{
    private readonly BaseContext _context;

    public CouponRepository(BaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
    {
        return await _context.Coupons.Include(c => c.MarketingUser).ToListAsync();
    }

    public async Task<IEnumerable<Coupon>> GetCouponsByDateRangeAsync(DateTime? startDate, DateTime? endDate)
    {
        IQueryable<Coupon> query = _context.Coupons.Include(c => c.MarketingUser);

        if (startDate.HasValue)
        {
            query = query.Where(c => c.ActivationDate >= startDate);
        }

        if (endDate.HasValue)
        {
            query = query.Where(c => c.expiration_date <= endDate);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Coupon>> GetCouponsByCreatorNameAsync(string creatorName)
    {
        return await _context.Coupons.Include(c => c.MarketingUser)
                                     .Where(c => c.MarketingUser.Username == creatorName)
                                     .ToListAsync();
    }

    public async Task<IEnumerable<Coupon>> GetCouponsByActivationDateAsync(DateTime activationDate)
    {
        return await _context.Coupons.Include(c => c.MarketingUser)
                                     .Where(c => c.ActivationDate == activationDate.Date)
                                     .ToListAsync();
    }

    public async Task<IEnumerable<Coupon>> GetCouponsByExpirationDateAsync(DateTime expirationDate)
    {
        return await _context.Coupons.Include(c => c.MarketingUser)
                                     .Where(c => c.expiration_date == expirationDate.Date)
                                     .ToListAsync();
    }

    public async Task<IEnumerable<Coupon>> GetCouponsActiveAsync()
    {
        return await _context.Coupons.Include(c => c.MarketingUser)
                                     .Where(c => c.status == "active")
                                     .ToListAsync();
    }

      public async Task AddCouponAsync(Coupon coupon)
      {
          using (var transaction = await _context.Database.BeginTransactionAsync())
          {
              try
              {
                  await _context.Coupons.AddAsync(coupon);
                  await _context.SaveChangesAsync();

                  // Agregar entrada en CouponHistory
                  var couponHistory = new CouponHistory
                  {
                      CouponId = coupon.id,
                      ChangeDate = DateTime.UtcNow,
                      FieldChanged = "Created",
                      OldValue = coupon.DiscountValue,
                      NewValue = "Coupon Created"
                  };

                  await _context.CouponHistories.AddAsync(couponHistory);
                  await _context.SaveChangesAsync();

                  await transaction.CommitAsync();
              }
              catch (Exception)
              {
                  await transaction.RollbackAsync();
                  throw;
              }
          }
      }


    public async Task<Coupon> GetByIdAsync(int id)
    {
        return await _context.Coupons.SingleOrDefaultAsync(c => c.id == id);
    }

    public async Task UpdateCouponAsync(Coupon couponEntity)
    {
        var existingCoupon = await GetByIdAsync(couponEntity.id);

        if (existingCoupon == null)
        {
            throw new Exception("Cupón no encontrado.");
        }

        // Verifica si el cupón ha sido redimido
        if (existingCoupon.status == "redimido")
        {
            throw new Exception("El cupón no se puede editar porque ya ha sido utilizado.");
        }

        // Mapea las propiedades desde couponEntity a existingCoupon
        _context.Entry(existingCoupon).CurrentValues.SetValues(couponEntity);

        // Realiza el guardado en la base de datos
        await _context.SaveChangesAsync();

        // Agregar entrada en CouponHistory
        var couponHistory = new CouponHistory
        {
            CouponId = existingCoupon.id,
            ChangeDate = DateTime.UtcNow,
            FieldChanged = "Updated",
            OldValue = existingCoupon.DiscountValue, 
            NewValue = "Coupon Updated"
        };

        await _context.CouponHistories.AddAsync(couponHistory);
        await _context.SaveChangesAsync();
    }



}
