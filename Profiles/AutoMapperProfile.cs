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
       .ForMember(dest => dest.MarketingUsername, opt => opt.MapFrom(src => src.MarketingUser.Username))
       .ForMember(dest => dest.quantity_uses, opt => opt.MapFrom(src => src.quantity_uses))
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

       CreateMap<CuponUpdateDto, Coupon>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
     .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
     .ForMember(dest => dest.ActivationDate, opt => opt.MapFrom(src => src.ActivationDate))
     .ForMember(dest => dest.expiration_date, opt => opt.MapFrom(src => src.expiration_date))
     .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.DiscountType))
     .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.DiscountValue))
     .ForMember(dest => dest.UseType, opt => opt.MapFrom(src => src.UseType))
     .ForMember(dest => dest.quantity_uses, opt => opt.MapFrom(src => src.quantity_uses))
     .ForMember(dest => dest.MinPurchaseAmount, opt => opt.MapFrom(src => src.MinPurchaseAmount))
     .ForMember(dest => dest.MaxPurchaseAmount, opt => opt.MapFrom(src => src.MaxPurchaseAmount))
     .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.status))
     .ForMember(dest => dest.RedemptionLimit, opt => opt.MapFrom(src => src.RedemptionLimit))
     .ForMember(dest => dest.MarketingUserid, opt => opt.MapFrom(src => src.MarketingUserid));



      // Mapeo para CouponDetailDto
      CreateMap<Coupon, CouponDetailDto>()
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
          .ForMember(dest => dest.Activation_date, opt => opt.MapFrom(src => src.ActivationDate))
          .ForMember(dest => dest.expiration_date, opt => opt.MapFrom(src => src.expiration_date))
          .ForMember(dest => dest.Discount_value, opt => opt.MapFrom(src => src.DiscountValue))
          .ForMember(dest => dest.Current_redemptions, opt => opt.MapFrom(src => src.CurrentRedemptions))
          .ForMember(dest => dest.MarketingUsername, opt => opt.MapFrom(src => src.MarketingUser.Username));

      CreateMap<CouponCreateDto, Coupon>()
     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
     .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
     .ForMember(dest => dest.ActivationDate, opt => opt.MapFrom(src => src.ActivationDate))
     .ForMember(dest => dest.expiration_date, opt => opt.MapFrom(src => src.expiration_date))
     .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.DiscountType))
     .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.DiscountValue))
     .ForMember(dest => dest.UseType, opt => opt.MapFrom(src => src.UseType))
     .ForMember(dest => dest.quantity_uses, opt => opt.MapFrom(src => src.quantity_uses))
     .ForMember(dest => dest.MinPurchaseAmount, opt => opt.MapFrom(src => src.MinPurchaseAmount))
     .ForMember(dest => dest.MaxPurchaseAmount, opt => opt.MapFrom(src => src.MaxPurchaseAmount))
     .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.status))
     .ForMember(dest => dest.RedemptionLimit, opt => opt.MapFrom(src => src.RedemptionLimit))
     .ForMember(dest => dest.MarketingUserid, opt => opt.MapFrom(src => src.MarketingUserid));

      CreateMap<MarketingUser, MarketingUserDto>()
      .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
      CreateMap<CouponHistory, CouponHistoryDto>()
            .ForMember(dest => dest.CouponName, opt => opt.MapFrom(src => src.Coupon.Name))
            .ForMember(dest => dest.MarketingUsername, opt => opt.MapFrom(src => src.MarketingUser.Username));
        }
        
    }
}
