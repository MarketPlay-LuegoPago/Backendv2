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
            CreateMap<Coupon, CouponsDto>()
            .ForMember(dest => dest.MarketingUsername, opt => opt.MapFrom(src => src.MarketingUser.Username));

            // Mapeo para CouponDetailDto
            CreateMap<Coupon, CouponDetailDto>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.activation_date, opt => opt.MapFrom(src => src.activation_date))
                .ForMember(dest => dest.expiration_date, opt => opt.MapFrom(src => src.expiration_date))
                .ForMember(dest => dest.discount_value, opt => opt.MapFrom(src => src.discount_value))
                .ForMember(dest => dest.current_redemptions, opt => opt.MapFrom(src => src.current_redemptions))
                .ForMember(dest => dest.MarketingUsername, opt => opt.MapFrom(src => src.MarketingUser.Username));
        }
    }
}
