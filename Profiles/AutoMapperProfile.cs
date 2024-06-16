using AutoMapper;
using Backengv2.Models;
using Backengv2.Dtos;

namespace Backengv2.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapeo para CouponsDto (si es necesario)
            CreateMap<Coupon, CouponsDto>();
            CreateMap<Coupon, CuponUpdateDto>();
            // Mapeo para CouponDetailDto
        
        }
    }
}