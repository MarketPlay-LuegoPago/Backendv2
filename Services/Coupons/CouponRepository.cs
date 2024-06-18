using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Data;
using Backengv2.Models;
using Microsoft.EntityFrameworkCore;
using Backengv2.Services;
using Backengv2.Dtos;
using AutoMapper;

namespace Backengv2.Services.Coupons
{
    public class CouponRepository : ICouponRepository
{
    private readonly BaseContext _context;
    private readonly IMarketplaceUserRepository  _marketplaceUserRepository;
    private readonly MailerSendService _mailerSendService;
    private readonly IMapper _mapper;

    public CouponRepository(IMapper mapper, BaseContext context,MailerSendService mailerSendService,IMarketplaceUserRepository marketplaceUserRepository)
    {
        _context = context;
        _mailerSendService = mailerSendService;
        _marketplaceUserRepository = marketplaceUserRepository;
        _mapper = mapper;
    
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

    public async Task<IEnumerable<Coupon>> GetCouponsByActivationDateAsync(DateTime activationDate)
    {
        return await _context.Coupons.Include(c => c.MarketingUser)
                                     .Where(c => c.ActivationDate == activationDate.Date)
                                     .ToListAsync();
    }


   public async Task AddCouponAsync(Coupon coupon)
{
    using (var transaction = await _context.Database.BeginTransactionAsync())
    {
        await _context.Coupons.AddAsync(coupon);
        await _context.SaveChangesAsync();

        var couponHistory = new CouponHistory
        {
            CouponId = coupon.id,
            ChangeDate = DateTime.UtcNow,
            FieldChanged = "Created",
            OldValue = coupon.DiscountValue.ToString(),
            NewValue = "Coupon Created",
            ChangedByUser = coupon.MarketingUserid
        };

        await _context.CouponHistories.AddAsync(couponHistory);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
    }
}




    public async Task<IEnumerable<CouponsDto>> GetCouponsForUserAsync(int userId, bool isAdmin)
      {
          IQueryable<Coupon> query = _context.Coupons.Include(c => c.MarketingUser);

          if (!isAdmin)
          {
              query = query.Where(c => c.MarketingUserid == userId);
          }

          var coupons = await query.ToListAsync();
          return _mapper.Map<IEnumerable<CouponsDto>>(coupons);
      }



    public async Task<IEnumerable<Coupon>> GetCouponsByCreatorNameAsync(string creatorName)
    {
        return await _context.Coupons.Include(c => c.MarketingUser)
                                     .Where(c => c.MarketingUser.Username == creatorName)
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

    public async Task<IEnumerable<CouponHistoryDto>> GetAllCouponHistoriesAsync()
    {
        var couponHistories = await _context.CouponHistories
            .Include(ch => ch.Coupon)
            .Include(ch => ch.MarketingUser)
            .ToListAsync();

        var couponHistoriesDto = _mapper.Map<IEnumerable<CouponHistoryDto>>(couponHistories);
        return couponHistoriesDto;
    }




   public async Task<Coupon> GetByIdAsync(int id)
    {
        return await _context.Coupons.Include(c => c.MarketingUser).SingleOrDefaultAsync(c => c.id == id);
    }

     public async Task UpdateCouponAsync(Coupon couponEntity)
    {
        var existingCoupon = await GetByIdAsync(couponEntity.id);

        if (existingCoupon == null)
        {
            throw new KeyNotFoundException("Cupón no encontrado.");
        }

        if (existingCoupon.status == "redimido")
        {
            throw new InvalidOperationException("El cupón no se puede editar porque ya ha sido utilizado.");
        }

        var changes = GetChanges(existingCoupon, couponEntity);
        await SaveChangesHistory(couponEntity.id, changes, couponEntity.MarketingUserid);

        _context.Entry(existingCoupon).CurrentValues.SetValues(couponEntity);
        await _context.SaveChangesAsync();
    }
        private Dictionary<string, (string, string)> GetChanges(Coupon existingCoupon, Coupon updatedCoupon)
        {
            var changes = new Dictionary<string, (string, string)>();

            foreach (var prop in typeof(Coupon).GetProperties())
            {
                var existingValue = prop.GetValue(existingCoupon);
                var updatedValue = prop.GetValue(updatedCoupon);

                if (!EqualityComparer<object>.Default.Equals(existingValue, updatedValue))
                {
                    changes.Add(prop.Name, (existingValue?.ToString(), updatedValue?.ToString()));
                }
            }

            return changes;
        }

    private async Task SaveChangesHistory(int couponId, Dictionary<string, (string, string)> changes, int userId)
    {
        foreach (var change in changes)
        {
            await _context.CouponHistories.AddAsync(new CouponHistory
            {
                CouponId = couponId,
                ChangeDate = DateTime.UtcNow,
                FieldChanged = change.Key,
                OldValue = change.Value.Item1,
                NewValue = change.Value.Item2,
                ChangedByUser = userId
            });
        }
        await _context.SaveChangesAsync();
    }

    // Otros métodos y lógica del repositorio...







    

   









     public async Task<bool> SendCouponToCustomersAsync(int couponId, string message, List<int> marketplaceUserIds)
    {
        var coupon = await GetByIdAsync(couponId);

        if (coupon == null || marketplaceUserIds == null || !marketplaceUserIds.Any())
        {
            throw new ArgumentException("Cupón o usuarios de  MarketplaceUser no encontrados.");
        }

        var marketplaceUsers = await _marketplaceUserRepository.GetMarketplaceUsersByIdsAsync(marketplaceUserIds);
        if (marketplaceUsers == null || !marketplaceUsers.Any())
        {
            throw new ArgumentException("Usuarios de MarketplaceUser no encontrados.");
        }

        bool allEmailsSent = true;

        foreach (var user in marketplaceUsers)
        {
            var emailContent = $"<p>Nombre del cupón: {coupon.Name}</p>" +
                               $"<p>Descripción: {coupon.Description}</p>" +
                               $"<p>Fecha de vigencia: {coupon.expiration_date}</p>" +
                               $"<p>Valor del descuento: {coupon.DiscountValue}</p>" +
                               (message != null ? $"<p>{message}</p>" : string.Empty);

            try
            {
                await _mailerSendService.SendEmailAsync("MS_zrwFkg@trial-ynrw7gynq8r42k8e.mlsender.net", "Sistecredito", new List<string> { user.Email }, new List<string> { user.Username }, "Cupón Enviado", "", emailContent);
            }
            catch
            {
                allEmailsSent = false; // Al menos uno falló
            }
        }

        return allEmailsSent;
    }


     public async Task<Coupon?> GetById(int id)
        {
            return await _context.Coupons.FindAsync(id);
        }

        public async Task DeleteCouponAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();
        }

public async Task statuschangeCouponAsync(Coupon coupon)
{
    if (_context == null)
    {
        throw new InvalidOperationException("El contexto de la base de datos no está configurado correctamente.");
    }

    // Obtener el estado anterior para el historial de cambio
    var couponInDb = await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.id == coupon.id);
    if (couponInDb == null)
    {
        throw new InvalidOperationException("Cupón no encontrado en la base de datos.");
    }

    // Actualiza el cupón en la base de datos
    _context.Coupons.Update(coupon);
    await _context.SaveChangesAsync();

    // Registro de la historia del cambio
    var couponHistory = new CouponHistory
    {
        CouponId = coupon.id,
        ChangeDate = DateTime.UtcNow,
        FieldChanged = "status",
        OldValue = couponInDb.status, // Estado anterior
        NewValue = coupon.status,      // Nuevo estado
        ChangedByUser = coupon.MarketingUserid
    };

    // Almacenando la historia del cambio
    try
    {
        await _context.CouponHistories.AddAsync(couponHistory);
        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException("Error al registrar el cambio de estado del cupón.", ex);
    }
}

        public async Task<bool> IsCouponRedeemedAsync(int userId, int couponId)
            {
                return await _context.CouponUsages
                    .AnyAsync(u => u.userId == userId && u.CouponId == couponId);
            }

            public async Task AddCouponUsageAsync(CouponUsage usage)
            {
                await _context.CouponUsages.AddAsync(usage);
                await _context.SaveChangesAsync();
            }

    public async Task<Coupon> GetCouponByIdAsync(int couponId)
    {
        return await _context.Coupons.FindAsync(couponId);
    }



        /* 
             public async Task<Coupon?> GetById(int id)
                {
                    return await _context.Coupons.FindAsync(id);
                }

                public async Task DeleteCouponAsync(Coupon coupon)
                {
                    _context.Coupons.Update(coupon);
                    await _context.SaveChangesAsync();
                }

        public async Task statuschangeCouponAsync(Coupon coupon)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("El contexto de la base de datos no está configurado correctamente.");
            }

            // Obtener el estado anterior para el historial de cambio
            var couponInDb = await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.id == coupon.id);
            if (couponInDb == null)
            {
                throw new InvalidOperationException("Cupón no encontrado en la base de datos.");
            }

            // Actualiza el cupón en la base de datos
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();

            // Registro de la historia del cambio
            var couponHistory = new CouponHistory
            {
                CouponId = coupon.id,
                ChangeDate = DateTime.UtcNow,
                FieldChanged = "status",
                OldValue = couponInDb.status, // Estado anterior
                NewValue = coupon.status,      // Nuevo estado
                ChangedByUser = coupon.MarketingUserId
            };

            // Almacenando la historia del cambio
            try
            {
                await _context.CouponHistories.AddAsync(couponHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al registrar el cambio de estado del cupón.", ex);
            }
        }

                public async Task<bool> IsCouponRedeemedAsync(int userId, int couponId)
                    {
                        return await _context.CouponUsages
                            .AnyAsync(u => u.userId == userId && u.CouponId == couponId);
                    }

                    public async Task AddCouponUsageAsync(CouponUsage usage)
                    {
                        await _context.CouponUsages.AddAsync(usage);
                        await _context.SaveChangesAsync();
                    }

            public async Task<Coupon> GetCouponByIdAsync(int couponId)
            {
                return await _context.Coupons.FindAsync(couponId);
            }


                //Clase para mostrar Cupones por
                public Task<IEnumerable<Coupon>> GetCouponsByJwtAsync(int id)
                {
                    throw new NotImplementedException();
                } */
    }

}


