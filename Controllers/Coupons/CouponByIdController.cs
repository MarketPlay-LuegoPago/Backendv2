
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace Backengv2.Controllers
{
    [Route("api/[Controller")]
    [Authorize]

    public class CouponByIdController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        public CouponByIdController (ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository; 
        }
        //Logica para mostrar cupones
        
    }
}