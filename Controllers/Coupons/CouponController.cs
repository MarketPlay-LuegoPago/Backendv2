using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Data;
using Microsoft.EntityFrameworkCore;
using Backengv2.Models;

namespace Backengv2.Controllers // Define el espacio de nombres para el controlador
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador
    public class CouponController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        // Campos privados para el contexto de la base de datos y el mapeador de objetos
        private readonly BaseContext _context;
        private readonly IMapper _mapper;

        // Constructor que inyecta el contexto de la base de datos y el mapeador de objetos
        public CouponController(BaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Acción HTTP GET para obtener una lista de cupones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponsDto>>> GetCoupons()
        {
            // Obtiene la lista de cupones incluyendo el usuario de marketing relacionado
            var coupons = await _context.Coupons.Include(c => c.MarketingUser).ToListAsync();
            // Mapea los cupones a DTOs (Data Transfer Objects)
            var couponsDto = _mapper.Map<IEnumerable<CouponsDto>>(coupons);
            // Devuelve la lista de cupones como respuesta HTTP 200 OK
            return Ok(couponsDto);
        }
    }
}
