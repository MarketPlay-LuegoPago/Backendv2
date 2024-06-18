using AutoMapper;
using Backengv2.Models;
using Backengv2.Dtos;

namespace Backengv2.Profiles
{
    public class AutoMapperProfile : Profile
    {
    public AutoMapperProfile()
    {
         // Configuración de mapeos entre entidades y DTOs usando AutoMapper

            // Mapeo de Coupon a CouponsDto
            CreateMap<Coupon, CouponsDto>()
                .ForMember(dest => dest.MarketingUsername, opt => opt.MapFrom(src => src.MarketingUser.Username)) // Mapea el nombre de usuario del marketing
                .ForMember(dest => dest.quantity_uses, opt => opt.MapFrom(src => src.quantity_uses)) // Mapea la cantidad de usos
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)); // Mapea el nombre

            // Mapeo de Coupon a CuponUpdateDto
            CreateMap<Coupon, CuponUpdateDto>();

            // Mapeo de Coupon a CouponDetailDto
            CreateMap<Coupon, CouponDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Mapea el nombre
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)) // Mapea la descripción
                .ForMember(dest => dest.Activation_date, opt => opt.MapFrom(src => src.ActivationDate)) // Mapea la fecha de activación
                .ForMember(dest => dest.expiration_date, opt => opt.MapFrom(src => src.expiration_date)) // Mapea la fecha de expiración
                .ForMember(dest => dest.Discount_value, opt => opt.MapFrom(src => src.DiscountValue)) // Mapea el valor del descuento
                .ForMember(dest => dest.Current_redemptions, opt => opt.MapFrom(src => src.CurrentRedemptions)) // Mapea las redenciones actuales
                .ForMember(dest => dest.MarketingUsername, opt => opt.MapFrom(src => src.MarketingUser.Username)); // Mapea el nombre de usuario del marketing

            // Mapeo de CouponCreateDto a Coupon
            CreateMap<CouponCreateDto, Coupon>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Mapea el nombre
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)) // Mapea la descripción
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate)) // Mapea la fecha de creación
                .ForMember(dest => dest.ActivationDate, opt => opt.MapFrom(src => src.ActivationDate)) // Mapea la fecha de activación
                .ForMember(dest => dest.expiration_date, opt => opt.MapFrom(src => src.expiration_date)) // Mapea la fecha de expiración
                .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.DiscountType)) // Mapea el tipo de descuento
                .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.DiscountValue)) // Mapea el valor del descuento
                .ForMember(dest => dest.UseType, opt => opt.MapFrom(src => src.UseType)) // Mapea el tipo de uso
                .ForMember(dest => dest.quantity_uses, opt => opt.MapFrom(src => src.QuantityUses)) // Mapea la cantidad de usos
                .ForMember(dest => dest.MinPurchaseAmount, opt => opt.MapFrom(src => src.MinPurchaseAmount)) // Mapea el monto mínimo de compra
                .ForMember(dest => dest.MaxPurchaseAmount, opt => opt.MapFrom(src => src.MaxPurchaseAmount)) // Mapea el monto máximo de compra
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.status)) // Mapea el estado
                .ForMember(dest => dest.RedemptionLimit, opt => opt.MapFrom(src => src.RedemptionLimit)) // Mapea el límite de redención
                .ForMember(dest => dest.MarketingUserid, opt => opt.MapFrom(src => src.MarketingUserid)); // Mapea el ID del usuario de marketing

            // Mapeo de MarketingUser a MarketingUserDto
            CreateMap<MarketingUser, MarketingUserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username)); // Mapea el nombre de usuario

            // Mapeo de CouponHistory a CouponHistoryDto
            CreateMap<CouponHistory, CouponHistoryDto>()
                .ForMember(dest => dest.CouponName, opt => opt.MapFrom(src => src.Coupon.Name)) // Mapea el nombre del cupón
                .ForMember(dest => dest.MarketingUsername, opt => opt.MapFrom(src => src.MarketingUser.Username)); // Mapea el nombre de usuario del marketing
        }
        
    }
}
