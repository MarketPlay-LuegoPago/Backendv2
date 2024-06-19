using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Services.Coupons;
using System;
using System.Security.Claims;

namespace Backengv2.Controllers // Define el espacio de nombres para el controlador
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    public class CouponFilterController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        // Campos privados para el repositorio de cupones y el mapeador de objetos
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        // Constructor que inyecta el repositorio de cupones y el mapeador de objetos
        public CouponFilterController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        // Acción HTTP GET para obtener cupones dentro de un rango de fechas
        [HttpGet("byDateRange")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsByDateRange(
            DateTime? startDate = null, // Fecha de inicio opcional
            DateTime? endDate = null) // Fecha de fin opcional
        {
            var coupons = await _couponRepository.GetCouponsByDateRangeAsync(startDate, endDate);
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        // Acción HTTP GET para obtener cupones por nombre del creador
        [HttpGet("byCreatorName")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsByCreatorName([FromQuery] string creatorName)
        {
            var coupons = await _couponRepository.GetCouponsByCreatorNameAsync(creatorName);
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        // Acción HTTP GET para obtener cupones por fecha de activación
        [HttpGet("byActivationDate")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsByActivationDate([FromQuery] DateTime activationDate)
        {
            var coupons = await _couponRepository.GetCouponsByActivationDateAsync(activationDate);
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        // Acción HTTP GET para obtener cupones por fecha de expiración
        [HttpGet("byExpirationDate")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsByExpirationDate([FromQuery] DateTime expirationDate)
        {
            var coupons = await _couponRepository.GetCouponsByExpirationDateAsync(expirationDate);
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        // Acción HTTP GET para obtener cupones activos
        [HttpGet("byCouponActive")]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCouponsActive()
        {
            var coupons = await _couponRepository.GetCouponsActiveAsync();
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            return Ok(couponsDto);
        }

        // Acción HTTP GET para obtener historiales de cupones
        [HttpGet("couponHistories")]
        public async Task<ActionResult<IEnumerable<CouponHistoryDto>>> GetCouponHistories()
        {
            var couponHistories = await _couponRepository.GetAllCouponHistoriesAsync();
            return Ok(couponHistories);
        }

        // Acción HTTP GET para obtener cupones por ID del usuario de marketing
        [HttpGet("byMarketingUser/{marketingUserId}")]
        public async Task<ActionResult<List<CouponsDto>>> GetCouponsByMarketingUserId(int marketingUserId)
        {
            var coupons = await _couponRepository.GetCouponsByMarketingUserIdAsync(marketingUserId);
            if (coupons == null || coupons.Count == 0)
            {
                // Devuelve un error 404 si no se encuentran cupones
                return NotFound();
            }

            return Ok(coupons);
        }
    }
}
