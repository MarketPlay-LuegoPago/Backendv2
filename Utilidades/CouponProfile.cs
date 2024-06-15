using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backengv2.Dtos;
using Backengv2.Models;

namespace Backengv2.Utilidades
{

public class CouponProfile : Profile
{
    public CouponProfile()
    {
        CreateMap<CouponsDto, Coupon>()
            .ForMember(dest => dest.MarketingUserId, opt => opt.MapFrom(src => src.MarketingUserId))
            .ReverseMap(); 
    }
}
}