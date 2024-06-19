using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Data;
using Microsoft.EntityFrameworkCore;
using Backengv2.Models;

namespace Backengv2.Controllers.Coupons // Define el espacio de nombres para el controlador
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    public class CouponDetailController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        // Campos privados para el contexto de la base de datos y el mapeador de objetos
        private readonly BaseContext _context;
        private readonly IMapper _mapper;

        // Constructor que inyecta el contexto de la base de datos y el mapeador de objetos
        public CouponDetailController(BaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Acción HTTP GET para obtener los detalles de un cupón por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CouponDetailDto>> GetCouponById(int id)
        {
            // Busca el cupón por su ID, incluyendo el usuario de marketing relacionado
            var coupon = await _context.Coupons
                .Include(c => c.MarketingUser) // Incluye la entidad MarketingUser relacionada
                .FirstOrDefaultAsync(c => c.id == id);

            if (coupon == null)
            {
                // Devuelve un error 404 si el cupón no se encuentra
                return NotFound();
            }

            // Mapea el cupón a un DTO de detalles del cupón
            var couponDto = _mapper.Map<CouponDetailDto>(coupon);
            // Devuelve los detalles del cupón como respuesta HTTP 200 OK
            return Ok(couponDto);
        }

        // Otros métodos de tu controlador pueden ir aquí
    }
}
