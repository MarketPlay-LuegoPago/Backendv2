
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace Backengv2.Controllers // Define el espacio de nombres para el controlador
{
    // Define la ruta base para el controlador y requiere autorización para acceder a él
    [Route("api/[Controller]")]
    [Authorize]
    public class CouponByIdController : ControllerBase // Hereda de ControllerBase para construir un controlador de API
    {
        // Campo privado para el repositorio de cupones
        private readonly ICouponRepository _couponRepository;

        // Constructor que inyecta el repositorio de cupones
        public CouponByIdController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository; 
        }

        // Aquí iría la lógica para mostrar cupones
    }
}
