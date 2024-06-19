using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backengv2.Data;
using Backengv2.Dtos;
using Backengv2.Models;
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Mvc;


namespace Backengv2.Controllers.Redemtion
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedemtionCouponController : ControllerBase
    {
        
        private readonly BaseContext _context;
        private readonly IMapper _mapper;
        private readonly ICouponRepository _couponRepository;

        public RedemtionCouponController(BaseContext context, IMapper mapper, ICouponRepository couponRepository)
        {
            _context = context;
            _mapper = mapper;
            _couponRepository = couponRepository;
        }

       [HttpPost]
[Route("redeem-coupon")]
public async Task<IActionResult> RedeemCoupon([FromBody] CouponRedemptionRequest request)
{
    // Obtener el cupón por ID y verificar si es válido.
    var coupon = await _couponRepository.GetCouponByIdAsync(request.couponId);
    if (coupon == null || coupon.status != "active" || coupon.expiration_date < DateTime.UtcNow)
    {
        return BadRequest("Cupón no válido o expirado.");
    }

    // Verificar si el cupón ya ha sido redimido por el usuario.
    var isAlreadyRedeemed = await _couponRepository.IsCouponRedeemedAsync(request.userId, request.couponId);
    if (isAlreadyRedeemed)
    {
        return BadRequest("Este cupón ya ha sido redimido.");
    }

    // Verificar si se ha alcanzado el límite de redenciones para el cupón.
/*     if (coupon.CurrentRedemptions >= coupon.RedemptionLimit)
    {
        return BadRequest("Límite de redenciones alcanzado para este cupón.");
    } */

    // Registrar la redención del cupón.
    var usage = new CouponUsage
    {
        CouponId = request.couponId,
        userId = request.userId,
        PurchaseId = request.PurchaseId,
        usage_date = DateTime.UtcNow,
        transaction_amount = request.transaction_amount,
        Status = "Redeemed"
    };

    await _couponRepository.AddCouponUsageAsync(usage);

    // Incrementar el número de redenciones del cupón.
    coupon.CurrentRedemptions++;
    await _couponRepository.UpdateCouponAsync(coupon);

    return Ok("Redención exitosa.");
}



        




}}