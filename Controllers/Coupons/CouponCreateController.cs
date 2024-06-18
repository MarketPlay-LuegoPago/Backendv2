using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Models;
using Backengv2.Services.Coupons;
using Backengv2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backengv2.Controllers.Coupons // Define el espacio de nombres para el controlador
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    public class CouponCreateController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        // Campos privados para el contexto de la base de datos, el repositorio de cupones, el mapeador de objetos y el logger
        private readonly BaseContext _context;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponCreateController> _logger;

        // Constructor que inyecta el contexto de la base de datos, el repositorio de cupones, el mapeador de objetos y el logger
        public CouponCreateController(BaseContext context, ICouponRepository couponRepository, IMapper mapper, ILogger<CouponCreateController> logger)
        { 
            _context = context;
            _couponRepository = couponRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // Acción HTTP POST para crear un nuevo cupón
        [HttpPost]
        public async Task<ActionResult<CouponsDto>> CreateCoupon([FromBody] CouponCreateDto couponCreateDto)
        {
            if (!ModelState.IsValid)
            {
                // Devuelve los errores de validación del modelo
                return BadRequest(ModelState);
            }

            // Busca el usuario de marketing por su ID
            var marketingUser = await _context.MarketingUsers.FirstOrDefaultAsync(u => u.id == couponCreateDto.MarketingUserid);
            if (marketingUser == null)
            {
                // Devuelve un error si el ID del usuario de marketing no es válido
                return BadRequest("El ID de usuario de marketing proporcionado no es válido.");
            }

            // Mapea el DTO del cupón a la entidad de cupón
            var couponEntity = _mapper.Map<Coupon>(couponCreateDto);
            // Establece la fecha de creación del cupón a la fecha y hora actual en UTC
            couponEntity.CreationDate = DateTime.UtcNow;
            // Asocia el usuario de marketing con el cupón
            couponEntity.MarketingUser = marketingUser;

            // Agrega el nuevo cupón al repositorio de cupones
            await _couponRepository.AddCouponAsync(couponEntity);

            // Obtiene el cupón creado desde la base de datos, incluyendo el usuario de marketing relacionado
            var createdCoupon = await _context.Coupons.Include(c => c.MarketingUser).FirstOrDefaultAsync(c => c.id == couponEntity.id);
            // Mapea el cupón creado a un DTO
            var createdCouponDto = _mapper.Map<CouponsDto>(createdCoupon);

            // Devuelve el cupón creado como respuesta HTTP 201 Created
            return CreatedAtAction(nameof(GetCouponById), new { id = createdCouponDto.Id }, createdCouponDto);
        }

        // Acción HTTP GET para obtener un cupón por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CouponsDto>> GetCouponById(int id)
        {
            // Busca el cupón por su ID en el repositorio de cupones
            var coupon = await _couponRepository.GetByIdAsync(id);

            if (coupon == null)
            {
                // Devuelve un error 404 si el cupón no se encuentra
                return NotFound();
            }

            // Mapea el cupón a un DTO
            var couponDto = _mapper.Map<CouponsDto>(coupon);
            // Devuelve el cupón como respuesta HTTP 200 OK
            return Ok(couponDto);
        }
    }
}
