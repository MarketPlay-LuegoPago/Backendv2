using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backengv2.Models;
using Backengv2.Dtos;

namespace Backengv2.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Coupon, CouponsDto>();
            CreateMap<CouponsDto, Coupon>();
        }
    }
}