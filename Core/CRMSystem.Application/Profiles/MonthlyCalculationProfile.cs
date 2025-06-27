using AutoMapper;
using CRMSystem.Application.Dtos.Calculation;
using CRMSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Profiles
{
    public class MonthlyCalculationProfile : Profile
    {
        public MonthlyCalculationProfile()
        {
            CreateMap<MonthlyCalculation, CalculationDto>()
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company.Id.ToString()))
             .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<CreateOrUpdateCalculationDto, MonthlyCalculation>();
        }
    }
}
