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
    // Incluye la relación con MarketingUser y carga todos los cupones
    return await _context.Coupons.Include(c => c.MarketingUser).ToListAsync();
}

public async Task<IEnumerable<Coupon>> GetCouponsByDateRangeAsync(DateTime? startDate, DateTime? endDate)
{
    IQueryable<Coupon> query = _context.Coupons.Include(c => c.MarketingUser);

    // Filtra por fecha de activación si startDate tiene valor
    if (startDate.HasValue)
    {
        query = query.Where(c => c.ActivationDate >= startDate);
    }

    // Filtra por fecha de vencimiento si endDate tiene valor
    if (endDate.HasValue)
    {
        query = query.Where(c => c.expiration_date <= endDate);
    }

    // Retorna la lista de cupones filtrados
    return await query.ToListAsync();
}

   public async Task<IEnumerable<Coupon>> GetCouponsByActivationDateAsync(DateTime activationDate)
{
    // Incluye la relación con MarketingUser y filtra los cupones por fecha de activación
    return await _context.Coupons.Include(c => c.MarketingUser)
                                 .Where(c => c.ActivationDate == activationDate.Date)
                                 .ToListAsync();
}

public async Task AddCouponAsync(Coupon coupon)
{
    // Inicia una transacción para agregar el cupón y su historial
    using (var transaction = await _context.Database.BeginTransactionAsync())
    {
        // Agrega el cupón a la base de datos
        await _context.Coupons.AddAsync(coupon);
        await _context.SaveChangesAsync();

        // Crea un registro en el historial de cambios del cupón
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

        // Confirma la transacción
        await transaction.CommitAsync();
    }
}

   public async Task<List<CouponsDto>> GetCouponsByMarketingUserIdAsync(int marketingUserId)
{
    // Obtiene todos los cupones relacionados con un usuario de marketing específico y los mapea a CouponsDto
    var coupons = await _context.Coupons
        .Include(c => c.MarketingUser) // Incluye la entidad MarketingUser relacionada
        .Where(c => c.MarketingUserid == marketingUserId)
        .ToListAsync();

    return _mapper.Map<List<CouponsDto>>(coupons);
}

public async Task<IEnumerable<Coupon>> GetCouponsByCreatorNameAsync(string creatorName)
{
    // Obtiene todos los cupones creados por un usuario de marketing cuyo nombre coincide con el especificado
    return await _context.Coupons.Include(c => c.MarketingUser)
                                 .Where(c => c.MarketingUser.Username == creatorName)
                                 .ToListAsync();
}

public async Task<IEnumerable<Coupon>> GetCouponsByExpirationDateAsync(DateTime expirationDate)
{
    // Obtiene todos los cupones con una fecha de vencimiento específica y los incluye junto con su MarketingUser relacionado
    return await _context.Coupons.Include(c => c.MarketingUser)
                                 .Where(c => c.expiration_date == expirationDate.Date)
                                 .ToListAsync();
}

   public async Task<IEnumerable<Coupon>> GetCouponsActiveAsync()
{
    // Obtiene todos los cupones activos y los incluye junto con su MarketingUser relacionado
    return await _context.Coupons.Include(c => c.MarketingUser)
                                 .Where(c => c.status == "active")
                                 .ToListAsync();
}

public async Task<IEnumerable<CouponHistoryDto>> GetAllCouponHistoriesAsync()
{
    // Obtiene todos los historiales de cupones, incluyendo las entidades Coupon y MarketingUser relacionadas, y los mapea a CouponHistoryDto
    var couponHistories = await _context.CouponHistories
        .Include(ch => ch.Coupon)
        .Include(ch => ch.MarketingUser)
        .ToListAsync();

    var couponHistoriesDto = _mapper.Map<IEnumerable<CouponHistoryDto>>(couponHistories);
    return couponHistoriesDto;
}

public async Task<Coupon> GetByIdAsync(int id)
{
    // Obtiene un cupón por su ID específico, incluyendo su MarketingUser relacionado
    return await _context.Coupons.Include(c => c.MarketingUser).SingleOrDefaultAsync(c => c.id == id);
}

    public async Task UpdateCouponAsync(Coupon couponEntity)
{
    // Obtiene el cupón existente por su ID
    var existingCoupon = await GetByIdAsync(couponEntity.id);

    // Verifica si el cupón existe
    if (existingCoupon == null)
    {
        throw new KeyNotFoundException("Cupón no encontrado.");
    }

    // Verifica si el cupón ya ha sido redimido y no se puede editar
    if (existingCoupon.status == "redimido")
    {
        throw new InvalidOperationException("El cupón no se puede editar porque ya ha sido utilizado.");
    }

    // Obtiene los cambios entre el cupón existente y el cupón actualizado
    var changes = GetChanges(existingCoupon, couponEntity);

    // Guarda la historia de cambios
    await SaveChangesHistory(couponEntity.id, changes, couponEntity.MarketingUserid);

    // Actualiza los valores del cupón existente con los del cupón actualizado
    _context.Entry(existingCoupon).CurrentValues.SetValues(couponEntity);

    // Guarda los cambios en la base de datos
    await _context.SaveChangesAsync();
}

// Método privado para obtener los cambios entre dos instancias de Coupon
private Dictionary<string, (string, string)> GetChanges(Coupon existingCoupon, Coupon updatedCoupon)
{
    var changes = new Dictionary<string, (string, string)>();

    // Itera sobre todas las propiedades de la clase Coupon
    foreach (var prop in typeof(Coupon).GetProperties())
    {
        var existingValue = prop.GetValue(existingCoupon);
        var updatedValue = prop.GetValue(updatedCoupon);

        // Compara los valores de las propiedades
        if (!EqualityComparer<object>.Default.Equals(existingValue, updatedValue))
        {
            // Agrega el nombre de la propiedad y sus valores antes y después del cambio
            changes.Add(prop.Name, (existingValue?.ToString(), updatedValue?.ToString()));
        }
    }

    return changes;
}


   private async Task SaveChangesHistory(int couponId, Dictionary<string, (string, string)> changes, int userId)
{
    // Itera sobre cada cambio registrado
    foreach (var change in changes)
    {
        // Crea un nuevo registro de historial de cupón con los detalles del cambio
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
    
    // Guarda todos los cambios en la base de datos
    await _context.SaveChangesAsync();
}

  
  public async Task<bool> SendCouponToCustomersAsync(int couponId, string message, List<int> marketplaceUserIds)
{
    // Obtiene el cupón con el ID proporcionado
    var coupon = await GetByIdAsync(couponId);

    // Verifica si el cupón o la lista de IDs de usuarios de MarketplaceUser son inválidos
    if (coupon == null || marketplaceUserIds == null || !marketplaceUserIds.Any())
    {
        throw new ArgumentException("Cupón o usuarios de MarketplaceUser no encontrados.");
    }

    // Obtiene los usuarios de MarketplaceUser por los IDs proporcionados
    var marketplaceUsers = await _marketplaceUserRepository.GetMarketplaceUsersByIdsAsync(marketplaceUserIds);

    // Verifica si no se encontraron usuarios de MarketplaceUser
    if (marketplaceUsers == null || !marketplaceUsers.Any())
    {
        throw new ArgumentException("Usuarios de MarketplaceUser no encontrados.");
    }

    bool allEmailsSent = true;

    // Itera sobre cada usuario de MarketplaceUser para enviarles el cupón por correo electrónico
    foreach (var user in marketplaceUsers)
    {
        // Construye el contenido del correo electrónico con los detalles del cupón y el mensaje opcional
        var emailContent = $"<p>Nombre del cupón: {coupon.Name}</p>" +
                           $"<p>Descripción: {coupon.Description}</p>" +
                           $"<p>Fecha de vigencia: {coupon.expiration_date}</p>" +
                           $"<p>Valor del descuento: {coupon.DiscountValue}</p>" +
                           (message != null ? $"<p>{message}</p>" : string.Empty);

        try
        {
            // Envía el correo electrónico a cada usuario de MarketplaceUser
            await _mailerSendService.SendEmailAsync("MS_zrwFkg@trial-ynrw7gynq8r42k8e.mlsender.net", "Sistecredito", new List<string> { user.Email }, new List<string> { user.Username }, "Cupón Enviado", "", emailContent);
        }
        catch
        {
            allEmailsSent = false; // Indica que al menos uno de los correos electrónicos falló
        }
    }

    // Devuelve true si todos los correos electrónicos fueron enviados exitosamente, de lo contrario, false
    return allEmailsSent;
}


    public async Task<Coupon?> GetById(int id)
{
    // Busca y devuelve el cupón con el ID especificado
    return await _context.Coupons.FindAsync(id);
}

public async Task DeleteCouponAsync(Coupon coupon)
{
    // Actualiza y guarda los cambios en la entidad de cupón proporcionada
    _context.Coupons.Update(coupon);
    await _context.SaveChangesAsync();
}

public async Task statuschangeCouponAsync(Coupon coupon)
{
    // Verifica si el contexto de la base de datos está configurado correctamente
    if (_context == null)
    {
        throw new InvalidOperationException("El contexto de la base de datos no está configurado correctamente.");
    }

    // Obtiene el estado anterior del cupón para el historial de cambios
    var couponInDb = await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.id == coupon.id);
    if (couponInDb == null)
    {
        throw new InvalidOperationException("Cupón no encontrado en la base de datos.");
    }

    // Actualiza el cupón en la base de datos
    _context.Coupons.Update(coupon);
    await _context.SaveChangesAsync();

    // Registra la historia del cambio de estado del cupón
    var couponHistory = new CouponHistory
    {
        CouponId = coupon.id,
        ChangeDate = DateTime.UtcNow,
        FieldChanged = "status",
        OldValue = couponInDb.status, // Estado anterior
        NewValue = coupon.status,     // Nuevo estado
        ChangedByUser = coupon.MarketingUserid
    };

    // Almacena la historia del cambio en la tabla de historial de cupones
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
    // Verifica si hay registros de uso del cupón para el usuario especificado
    return await _context.CouponUsages
        .AnyAsync(u => u.userId == userId && u.CouponId == couponId);
}

public async Task AddCouponUsageAsync(CouponUsage usage)
{
    // Agrega un nuevo registro de uso de cupón a la base de datos
    await _context.CouponUsages.AddAsync(usage);
    await _context.SaveChangesAsync();
}

public async Task<Coupon> GetCouponByIdAsync(int couponId)
{
    // Busca y devuelve el cupón con el ID especificado
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


